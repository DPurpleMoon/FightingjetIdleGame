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

    private float incomeTimer = 0f;

    public event Action OnIdleStatsChanged;
    public GameObject manObj;

    private void Awake()
    {
        if (Instance == null) { Instance = this; LoadIdleData(); }
        else Destroy(gameObject);
    }

    private void Start()
    {
        Debug.Log("test");
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
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        string logTime = (string)SaveLoad.LoadGame("LogTime");
        if (logTime == null)
        {
            logTime = DateTime.Now.ToBinary().ToString();
        }
        if (!long.TryParse(logTime, out long temp))
        {
            logTime = DateTime.Now.ToBinary().ToString();
        }
        DateTime lastLogout = DateTime.FromBinary(Convert.ToInt64(logTime));
        double secondsPassed = (DateTime.Now - lastLogout).TotalSeconds;

        // Cap at 24 hours (86400s)
        if (secondsPassed > 86400) secondsPassed = 86400;
  
        // Only show if away for more than 1 minute (60s)
        if (secondsPassed > 60)
        {
            SaveLoad.SaveGame("LogTime", DateTime.Now.ToBinary().ToString());
            DistributeReward(secondsPassed);
        }
    }

    private void DistributeReward(double seconds)
    {
        double totalEarned = seconds * GetCoinsPerSecond();
        CurrencyManager.Instance.AddCurrency(totalEarned);
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
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
    }
    
    private void OnApplicationPause(bool pause) 
    { 
        if (pause) OnApplicationQuit(); 
    }

    public void SaveIdleData()
    {
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        SaveLoad.SaveGame("IdleLevel", idleUpgradeLevel);
    }

    public void LoadIdleData()
    {
        manObj = GameObject.Find("SaveLoadManager");
        SaveLoadManager SaveLoad = manObj.GetComponent<SaveLoadManager>();
        idleUpgradeLevel = (int)SaveLoad.LoadGame("IdleLevel");
        if (idleUpgradeLevel == null)
        {
            idleUpgradeLevel = 0;
        }
        currentStage = (int)SaveLoad.LoadGame("CurrentStage");
        if (currentStage == null)
        {
            currentStage = 1;
        }
    }

    private void NotifyUI() => OnIdleStatsChanged?.Invoke();
}