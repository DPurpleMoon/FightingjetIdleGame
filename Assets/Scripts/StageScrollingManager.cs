using UnityEngine;

public class StageScrollingManager : MonoBehaviour {

    public StageScrollingController StageScript;

    private void Awake() 
    {
        StageScript = GetComponent<StageScrollingController>();
        StageScript.Initiate("stagedummy_0");
    }
    private void Start() 
    {
    }
}