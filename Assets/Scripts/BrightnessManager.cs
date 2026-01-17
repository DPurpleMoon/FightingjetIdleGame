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
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 1.0f);
        ApplyBrightness(savedBrightness);
    }
    
    void Start()
    {
        if (brightnessOverlay == null)
        {
            GameObject overlayObj = GameObject.Find("brightnessoverlay");
            if (overlayObj != null)
            {
                brightnessOverlay = overlayObj.GetComponent<Image>();
                Debug.Log("brightnessoverlay found!");
            }
        }
    }
    
    public void ApplyBrightness(float brightness)
    {
        if (brightnessOverlay != null)
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
            Debug.Log("Brightness: " + brightness + " | Color: " + overlayColor);
        }
    }
}