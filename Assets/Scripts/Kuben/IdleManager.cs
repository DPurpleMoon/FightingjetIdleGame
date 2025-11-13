using System;
using UnityEngine;

[DefaultExecutionOrder(-80)]
public class IdleManager : MonoBehaviour
{
    public static IdleManager Instance;

    public IdleData idleData;
    public int currentStage = 1;

    // Accumulator for awarding fractional seconds
    private float accumulator = 0f;

    private DateTime lastIdleDateTime;
    private const string PrefKey_LastIdleTime = "LastIdleTime";
    public int maxOfflineHours = 24; // cap (can be edited in inspector)

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        string lastSaved = PlayerPrefs.GetString(PrefKey_LastIdleTime, "");
        if (!string.IsNullOrEmpty(lastSaved))
        {
            if (DateTime.TryParse(lastSaved, out DateTime parsed))
                lastIdleDateTime = parsed;
            else
                lastIdleDateTime = DateTime.Now;
        }
        else lastIdleDateTime = DateTime.Now;

        AwardOfflineCurrency();
    }

    void Update()
    {
        if (idleData == null || CurrencyManager.Instance == null) return;

        // Compute effective rate: base rate depends on stage, ascension multiplier applied by CurrencyManager
        float baseRate = idleData.GetCurrentIdleRate(currentStage);

        // accumulate deltaTime * baseRate to determine fractional currency earned
        accumulator += Time.deltaTime * baseRate;

        if (accumulator >= 1f)
        {
            int award = Mathf.FloorToInt(accumulator);
            // Use CurrencyManager.AddCurrency(award) -> it will apply ascension multiplier automatically
            CurrencyManager.Instance.AddCurrency(award);
            accumulator -= award;
            // update lastIdleDateTime to now minus leftover fractional seconds for consistency
            lastIdleDateTime = DateTime.Now;
            PlayerPrefs.SetString(PrefKey_LastIdleTime, lastIdleDateTime.ToString());
            PlayerPrefs.Save();
        }
    }

    void OnApplicationQuit()
    {
        SaveLastIdleTime();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause) SaveLastIdleTime();
    }

    private void SaveLastIdleTime()
    {
        PlayerPrefs.SetString(PrefKey_LastIdleTime, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    // Offline awarding with cap
    public void AwardOfflineCurrency()
    {
        if (idleData == null || CurrencyManager.Instance == null) return;

        TimeSpan offlineSpan = DateTime.Now - lastIdleDateTime;
        double offlineSeconds = offlineSpan.TotalSeconds;

        if (offlineSeconds <= 0)
        {
            lastIdleDateTime = DateTime.Now;
            return;
        }

        // Apply cap
        double maxSeconds = maxOfflineHours * 3600;
        if (offlineSeconds > maxSeconds) offlineSeconds = maxSeconds;

        float baseRate = idleData.GetCurrentIdleRate(currentStage);

        // Compute raw offline currency (before ascension multiplier). We'll pass this raw amount to CurrencyManager.
        int offlineCurrency = Mathf.FloorToInt((float)(offlineSeconds * baseRate));
        if (offlineCurrency > 0)
            CurrencyManager.Instance.AddCurrency(offlineCurrency);

        lastIdleDateTime = DateTime.Now;
        PlayerPrefs.SetString(PrefKey_LastIdleTime, lastIdleDateTime.ToString());
        PlayerPrefs.Save();
    }
}
