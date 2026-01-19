using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager Instance;
    
    [Header("Brightness Overlay")]
    public Image brightnessOverlay;
    
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
    }
    
    void Start()
    {
        InitializeOverlay();
        
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 1.0f);
        ApplyBrightness(savedBrightness);
        
    }
    
    private void InitializeOverlay()
    {
        if (brightnessOverlay == null)
        {
            GameObject overlayObj = GameObject.Find("brightnessoverlay");
            if (overlayObj != null)
            {
                brightnessOverlay = overlayObj.GetComponent<Image>();
                Debug.Log("brightnessoverlay found!");
            }
            else
            {
                Debug.LogWarning("brightnessoverlay not found in scene!");
            }
        }
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        brightnessOverlay = null;
        InitializeOverlay();
        
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 1.0f);
        ApplyBrightness(savedBrightness);
    }
    
    public void ApplyBrightness(float brightness)
    {
        if (brightnessOverlay == null)
        {
            Debug.LogWarning("BrightnessManager: brightnessOverlay is null, cannot apply brightness");
            return;
        }
        
        try
        {
            Color overlayColor;
            
            if (brightness < 1.0f)
            {
                float alpha = 1.0f - brightness;
                overlayColor = new Color(0, 0, 0, alpha);
            }
            else if (brightness > 1.0f)
            {
                float alpha = (brightness - 1.0f);
                overlayColor = new Color(1, 1, 1, alpha);
            }
            else
            {
                overlayColor = new Color(0, 0, 0, 0);
            }
            
            brightnessOverlay.color = overlayColor;
            Debug.Log("Brightness applied: " + brightness + " | Color: " + overlayColor);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error applying brightness: " + e.Message);
        }
    }
}