using UnityEngine;
using TMPro;

public class statsUI : MonoBehaviour
{
    public GameObject[] statsslots;
    public CanvasGroup statscanvas;
    private bool isMenuOpen = false;

    private void Start()
    {
        UpdateDamage();
        UpdateMaxHealth();
        
        statscanvas.alpha = 0;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuOpen = !isMenuOpen;
            
            statscanvas.alpha = isMenuOpen ? 1 : 0;

            Time.timeScale = isMenuOpen ? 0 : 1;
        }
    }
    
    public void UpdateDamage()
    {
        statsslots[0].GetComponentInChildren<TextMeshProUGUI>().text = "Damage: " + Statsmanger.Instance.GetDamage();
    }

    public void UpdateMaxHealth()
    {
        statsslots[1].GetComponentInChildren<TextMeshProUGUI>().text = "MaxHealth: " + Statsmanger.Instance.GetMaxHealth();
    }
 
    public bool IsMenuOpen()
    {
        return isMenuOpen;
    }
}