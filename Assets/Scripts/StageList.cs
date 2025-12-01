using UnityEngine;
using UnityEngine.SceneManagement;

public class StageList : MonoBehaviour
{
    public void StagePressed()
    {
        SceneManager.LoadScene("Stage0");
    }
}
