using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-50)]
public class IdleManager : MonoBehaviour
{
    public static IdleManager Instance;

    [Header("Progression Data")]
    public int idleUpgradeLevel = 0;
    public int currentStage = 1;

    [Header("Stage Configuration")]
    public StageRead stageReader;
    public List<string> stageFileNames;
    [SerializeField] private float activeLevelMultiplier = 1.0f;

    [Header("Economy Settings")]
    public double baseRate = 0.1; 
    public double ratePerUpgrade = 0.12; // Reduced to make Upgrades supplementary
    public float incomeTicketInterval = 1.0f;

    [Header("Upgrade Costs")]
    public int upgradeBaseCost = 1800;
    public float upgradeCostMultiplier = 1.3f; // Slightly steeper curve

    [Header("UI References")]
    public GameObject welcomePanel;
    public TextMeshProUGUI welcomeText;

    private const string LogoutKey = "Idle_LogoutTime";
    private const string UpgradeKey = "Idle_UpgradeLevel";
    private const string StageKey = "Idle_CurrentStage";
    private float incomeTimer = 0f;

    public event Action OnIdleStatsChanged;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); LoadIdleData(); }
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateStageMultiplier();
        CalculateOfflineEarnings();
    }

    private void Update()
    {
        incomeTimer += Time.deltaTime;
        if (incomeTimer >= incomeTicketInterval)
        {
            double payout = GetCoinsPerSecond() * incomeTicketInterval;
            CurrencyManager.Instance.AddCurrency(payout);
            incomeTimer = 0f;
        }
    }

    public void UpdateStageMultiplier()
    {
        if (stageReader == null) stageReader = FindFirstObjectByType<StageRead>();
        
        int fileIndex = currentStage - 1;
        if (stageFileNames == null || stageFileNames.Count == 0) { activeLevelMultiplier = 1.0f; return; }
        
        // Clamp index
        if (fileIndex >= stageFileNames.Count) fileIndex = stageFileNames.Count - 1;
        if (fileIndex < 0) fileIndex = 0;

        string fileToLoad = stageFileNames[fileIndex];
        
        // Safety check if file exists
        if (stageReader != null)
            activeLevelMultiplier = stageReader.GetStageMultiplier(fileToLoad);
        else 
            activeLevelMultiplier = 1.0f;

        NotifyUI();
    }

    // Formula: (Base + (Level * 0.2)) * StageMultiplier * Ascension
    public double GetCoinsPerSecond()
    {
        double flatRate = baseRate + (idleUpgradeLevel * ratePerUpgrade);
        double total = flatRate * activeLevelMultiplier;

        if (AscensionManager.Instance != null)
            total *= AscensionManager.Instance.GetAscensionMultiplier();

        return total;
    }

    private void CalculateOfflineEarnings()
    {
        if (PlayerPrefs.HasKey(LogoutKey))
        {
            long temp = Convert.ToInt64(PlayerPrefs.GetString(LogoutKey));
            DateTime lastLogout = DateTime.FromBinary(temp);
            double secondsPassed = (DateTime.Now - lastLogout).TotalSeconds;

            // Cap at 24 hours (86400s)
            if (secondsPassed > 86400) secondsPassed = 86400;

            // Only show if away for more than 1 minute (60s)
            if (secondsPassed > 60)
            {
                DistributeReward(secondsPassed);
            }
        }
    }

    private void DistributeReward(double seconds)
    {
        double totalEarned = seconds * GetCoinsPerSecond();
        CurrencyManager.Instance.AddCurrency(totalEarned);

        if (welcomePanel != null && welcomeText != null && totalEarned > 0)
        {
            welcomePanel.SetActive(true);
            welcomeText.text = $"Welcome Back!\nTime Away: {seconds / 60:F0} mins\nEARNED: ${totalEarned:N0}";
        }
    }

    public double GetUpgradeCost()
    {
        return upgradeBaseCost * Math.Pow(upgradeCostMultiplier, idleUpgradeLevel);
    }

    public void BuyIdleUpgrade()
    {
        double cost = GetUpgradeCost();
        if (CurrencyManager.Instance.SpendCurrency(cost))
        {
            idleUpgradeLevel++;
            SaveIdleData();
            NotifyUI();
        }
    }

    public void AdvanceStage()
    {
        currentStage++;
        UpdateStageMultiplier();
        SaveIdleData();
        NotifyUI();

        // Load the actual Scene
        // Assumption: Scenes are named "Stage0", "Stage1"... index starts at 0.
        // currentStage 1 = "Stage0"
        string sceneName = "Stage" + (currentStage - 1);
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    [ContextMenu("Reset Idle")]
    public void ResetIdleProgress()
    {
        idleUpgradeLevel = 0;
        SaveIdleData();
        NotifyUI();
    }

    private void OnApplicationQuit() 
    { 
        SaveIdleData(); 
        PlayerPrefs.SetString(LogoutKey, DateTime.Now.ToBinary().ToString()); 
        PlayerPrefs.Save(); 
    }
    
    private void OnApplicationPause(bool pause) 
    { 
        if (pause) OnApplicationQuit(); 
    }

    public void SaveIdleData()
    {
        PlayerPrefs.SetInt(UpgradeKey, idleUpgradeLevel);
        PlayerPrefs.SetInt(StageKey, currentStage);
        PlayerPrefs.Save();
    }

    public void LoadIdleData()
    {
        idleUpgradeLevel = PlayerPrefs.GetInt(UpgradeKey, 0);
        currentStage = PlayerPrefs.GetInt(StageKey, 1);
    }

    private void NotifyUI() => OnIdleStatsChanged?.Invoke();
}