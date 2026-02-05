using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopReturn : MonoBehaviour
{
    public GameObject manObj;
    public void ShopClose()
    {
        manObj = GameObject.Find("Game Manager");
        CurrencyManager Currency = manObj.GetComponent<CurrencyManager>();
        Currency.SaveCurrency();
        SceneManager.LoadScene("StageList");
    }
}