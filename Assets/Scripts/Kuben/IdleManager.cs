using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem; //for the welcome UI

///<summary>
///Manages offline progression. Calculates earnings based on time away,
///stage progress, and purchased idle upgrades 
///</summary>
[DefaultExecutionOrder(-50)]
public class IdleManager : MonoBehaviour
{
    public static IdleManager Instance;

    [Header("Progression Data")]
    public int idleUpgradeLevel = 0; // Increases via button
    public int currentStage = 1;

    [Header("Stage Configuration")]
    public StageRead stageReader;
    public List<string> stageFileNames;

    [SerializeField] private float activeLevelMultiplier = 1.0f;

    [Header("Economy Settings")]
    public double baseRate = 0.1; // Base coins per second
    public double ratePerUpgrade = 0.1; // Added coins per second per upgrade
    public float incomeTicketInterval = 1.0f;

    [Header("Upgrade Costs")]
    public int upgradeBaseCost = 50;
    public float upgradeCostMultiplier = 1.1f;

    [Header("UI References")]
    public GameObject welcomePanel; // Popup panel for when the player enters the game after been away
    public TextMeshProUGUI welcomeText;

    private const string LogoutKey = "Idle_LogoutTime";
    private const string UpgradeKey = "Idle_UpgradeLevel";
    private const string StageKey = "Idle_CurrentStage"; // To sync with stage
    private float incomeTimer = 0f;

    // Event to update the Upgrade Button UI
    public event Action OnIdleStatsChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadIdleData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateStageMultiplier();
        CalculateOfflineEarnings();
    }

    // --- REAL-TIME PASSIVE INCOME
    private void Update()
    {
        incomeTimer += Time.deltaTime;
        if (incomeTimer >= incomeTicketInterval)
        {
            double earningPerSecond = GetCoinsPerSecond();
            double totalPayout = earningPerSecond * incomeTicketInterval;
            CurrencyManager.Instance.AddCurrency(totalPayout);
            incomeTimer = 0f;
        }

        // --- DEVELOPMENT TESTING CONTROLS ---
        // I: Reset Idle Progress
        // S: Increase Stage Level

        if (Keyboard.current == null) return;
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ResetIdleProgress();
            currentStage =1;
            UpdateStageMultiplier();
            Debug.Log($"[Debug] Idle Reset.");
        }
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            AdvanceStage();
        }
    }

    // --- OFFLINE CALCULATION ---

    // --- PUBLIC HELPERS ---


    public void UpdateStageMultiplier()
    {
        if (stageReader == null)
        {
            stageReader = FindFirstObjectByType<StageRead>();
            if (stageReader == null)
            {
                Debug.LogWarning("StageRead script missing from scene!");
                return;
            }
        }

        int fileIndex = currentStage - 1;

        if (stageFileNames == null || stageFileNames.Count == 0)
        {
            activeLevelMultiplier = 1.0f;
            return;
        }

        if (fileIndex >= stageFileNames.Count)
        {
            fileIndex = stageFileNames.Count - 1;
        }

        string fileToLoad = stageFileNames[fileIndex];

        activeLevelMultiplier = stageReader.GetStageMultiplier(fileToLoad);
        Debug.Log($"[IdleManager] Loaded Stage: {fileToLoad} | Multiplier: {activeLevelMultiplier}x");

        NotifyUI();
    }
    public double GetCoinsPerSecond()
    {
        //Formula: (Base + Upgrades) * (StageBonus ^ StageLevel)
        double flatRate = baseRate + (idleUpgradeLevel * ratePerUpgrade);
        double total = flatRate * activeLevelMultiplier;
        
        if (AscensionManager.Instance != null)
        {
            total *= AscensionManager.Instance.GetAscensionMultiplier();
        }

        return total;
    }

    // --- HELPERS --- 
        private void CalculateOfflineEarnings()
    {
        if (PlayerPrefs.HasKey(LogoutKey))
        {
            long temp = Convert.ToInt64(PlayerPrefs.GetString(LogoutKey));
            DateTime lastlogut = DateTime.FromBinary(temp);
            DateTime currenTime = DateTime.Now;

            TimeSpan difference = currenTime - lastlogut;
            double secondsPassed = difference.TotalSeconds;

            // Setting Cap for idle limit, 86400s = 24hrs 
            if (secondsPassed > 86400) secondsPassed = 86400;
            if (secondsPassed > 1200)
            {
                DistributeReward(secondsPassed);
            }
        }  
    }

    private void DistributeReward(double seconds)
    {
        double rate = GetCoinsPerSecond();
        double totalEarned = seconds * rate;

        // Apply to CurrencyManager
        CurrencyManager.Instance.AddCurrency(totalEarned);

        // UI
        if (welcomePanel != null && welcomeText != null && totalEarned > 15)
        {
            welcomePanel.SetActive(true);
            welcomeText.text = $"Welcome Back!\nTime Away: {seconds / 60:F0} mins\nRate: ${rate:F1}/sec\nEARNED: ${totalEarned:N0}";
        }
        Debug.Log($"[Idle System] Player earned {totalEarned} coins for {seconds} seconds offline.");
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

    // To call this function when player beats a stage 
    public void AdvanceStage()
    {
        currentStage++;
        UpdateStageMultiplier();
        SaveIdleData();
        NotifyUI();
        Debug.Log($"[Game Progression] Stage Advanced. Current Stage: {currentStage}");
    }

    // ---RESET FUNCTION
    [ContextMenu("Reset Idle Progress")]
    public void ResetIdleProgress()
    {
        idleUpgradeLevel = 0;
        // currentStage = 1:
        SaveIdleData();
        NotifyUI();
        Debug.Log("[Idle System] Progress Reset.");
    }

    // --- SAVE & lOAD ---

    private void OnApplicationQuit()
    {
        SaveIdleData();

        // To save time
        DateTime now = DateTime.Now;
        PlayerPrefs.SetString(LogoutKey, now.ToBinary().ToString());
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

    private void NotifyUI()
    {
        OnIdleStatsChanged?.Invoke();
    }
}