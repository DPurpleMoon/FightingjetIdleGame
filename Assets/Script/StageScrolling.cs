using UnityEngine;

public class StageScrolling : MonoBehaviour {
    public float ScrollCoordinate;
    public bool inStage = false;


    private void Update()
    {
        if (inStage == true) {
            ScrollCoordinate = ScrollCoordinate + 0.01f;
        }
    }
    
}
