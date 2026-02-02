using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopReturn : MonoBehaviour
{
    public void ShopClose()
    {
        SceneManager.LoadScene("StageList");
    }
}