using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageScrollingController : MonoBehaviour {
    public Vector2 TargetPosition;
    public Vector2 ActualLocation = new Vector2(0, 0);
    public bool StageCloned = false;
    public GameObject CurrentStage;
    public GameObject DupeStage;
    private float StageMoveVelocity;
    private float Multiplier;
    public float BGHeight;
    private Coroutine _scrollRoutine;

    public StageScrollingData Stage; 

    public void Scroll()
    {
        if (gameObject.name == Stage.StageName)
        {
            StartCoroutine(StageScrolling());
        }
    }

    private IEnumerator StageScrolling()
    {
    Multiplier = Stage.StartingVelocity;
    while (Mathf.Abs(ActualLocation.y - TargetPosition.y) > 0.01f)
        {
        if (Stage.inStage){
            // Acceleration method
            // Increase Multiplier if smaller than MaxVelocity
            if (Multiplier < Stage.MaxVelocity)
                {
                    Multiplier = Multiplier * Stage.AccelerationConstant; 
                }
            // Set Multiplier to MaxVelocity if exceed
            else if (Multiplier > Stage.MaxVelocity)
                {
                    Multiplier = Stage.MaxVelocity; 
                }
            // Dynamic deacceleration when ActualLocation is almost equals to TargetPostion
            else if (((ActualLocation.y - TargetPosition.y) < MathF.Log((Stage.MaxVelocity / Stage.StartingVelocity * (1 - Stage.AccelerationConstant)) - 1, 1 - Stage.AccelerationConstant)) && (Multiplier > Stage.StartingVelocity))
                {
                    Multiplier = Multiplier * (1 / Stage.AccelerationConstant); 
                }
            StageMoveVelocity = Multiplier * Time.deltaTime;
            ActualLocation = Vector2.MoveTowards(ActualLocation, TargetPosition, StageMoveVelocity);
            CurrentStage.transform.position = new Vector3(0, ((ActualLocation.y - TargetPosition.y) % BGHeight) - BGHeight, 0);
            DupeStage.transform.position = new Vector3(0, (ActualLocation.y - TargetPosition.y) % BGHeight, 0);
            yield return null;
            }
        }
        ActualLocation = TargetPosition;
    }

    public void Initiate(){
        CurrentStage = GameObject.Find(Stage.StageName);
        CurrentStage.SetActive(true);
        if (_scrollRoutine != null)
        {
            StopCoroutine(_scrollRoutine);
            _scrollRoutine = null;
        }
        if (DupeStage != null)
        {
            Destroy(DupeStage);
        }
        Renderer renderer = CurrentStage.GetComponent<Renderer>();
        BGHeight = renderer.bounds.size.y;
        ActualLocation = new Vector2(0, 0);
        TargetPosition = new Vector2(0, Stage.ScrollCoordinate);
        StageCloned = false;
        Stage.inStage = true;
        DupeStage = Instantiate(CurrentStage, new Vector3(0, BGHeight, 0), Quaternion.identity);
        StageCloned = true;
        _scrollRoutine = StartCoroutine(StageScrolling());
    
        // Clone the background once and put it on top of the original background
    }

    public void LeaveStage(){
        if (_scrollRoutine != null)
        {
            StopCoroutine(_scrollRoutine);
            _scrollRoutine = null;
        }
        if (DupeStage != null)
        {
            Destroy(DupeStage);
            DupeStage = null;
        }
        StageCloned = false;
        Stage.inStage = false;
        SceneManager.LoadScene("StageList");
        
    }
}

