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
    private float SafeBGHeight; // Backup value - moved here
    private Coroutine _scrollRoutine;
    public static bool isExiting = false;

    public StageScrollingData Stage; 

    private IEnumerator StageScrolling()
    {
        Multiplier = Stage.StartingVelocity;
        
        while (Mathf.Abs(ActualLocation.y - TargetPosition.y) > 0.01f && Stage.inStage)
        {
            if (!Stage.isPaused)
            {
                if (BGHeight <= 0 || float.IsNaN(BGHeight) || float.IsInfinity(BGHeight))
                {
                    Debug.LogWarning("BGHeight is invalid during scroll, using SafeBGHeight");
                    BGHeight = SafeBGHeight;
                }
                if (BGHeight <= 0)
                {
                    Debug.LogError("BGHeight is still 0! Skipping frame...");
                    yield return new WaitUntil(() => Time.timeScale > 0);
                    
                    continue;
                }
                if (Multiplier < Stage.MaxVelocity)
                {
                    Multiplier = Multiplier * Stage.AccelerationConstant; 
                }
                else if (Multiplier > Stage.MaxVelocity)
                {
                    Multiplier = Stage.MaxVelocity; 
                }
                else if (Multiplier > Stage.StartingVelocity)
                {
                    float distanceRemaining = Mathf.Abs(ActualLocation.y - TargetPosition.y);
                    float decelThreshold = Stage.MaxVelocity * 2f;
                    
                    if (distanceRemaining < decelThreshold)
                    {
                        float decelFactor = 1f / Stage.AccelerationConstant;
                        if (decelFactor > 0 && !float.IsInfinity(decelFactor) && !float.IsNaN(decelFactor))
                        {
                            Multiplier = Multiplier * decelFactor;
                        }
                    }
                }
                
                // Calculate velocity
                StageMoveVelocity = Multiplier * Time.deltaTime;
                
                // Safety check for velocity
                if (float.IsNaN(StageMoveVelocity) || float.IsInfinity(StageMoveVelocity))
                {
                    Debug.LogError("Invalid StageMoveVelocity! Resetting...");
                    StageMoveVelocity = Stage.StartingVelocity * Time.deltaTime;
                    Multiplier = Stage.StartingVelocity;
                }
                
                // Move towards target
                ActualLocation = Vector2.MoveTowards(ActualLocation, TargetPosition, StageMoveVelocity);
                
                // Calculate positions with safe BGHeight
                float yOffset = (ActualLocation.y - TargetPosition.y) % BGHeight;
                
                // Validate yOffset
                if (float.IsNaN(yOffset) || float.IsInfinity(yOffset))
                {
                    Debug.LogError("Invalid yOffset calculated!");
                    yield return new WaitUntil(() => Time.timeScale > 0);
                    
                    continue;
                }
                
                Vector3 currentPos = new Vector3(0, yOffset - BGHeight, 0);
                Vector3 dupePos = new Vector3(0, yOffset, 0);
                
                // Validate positions
                if (!IsValidPosition(currentPos) || !IsValidPosition(dupePos))
                {
                    Debug.LogError("Invalid positions calculated! Skipping frame...");
                    yield return new WaitUntil(() => Time.timeScale > 0);
                    
                    continue;
                }
                
                // Assign positions safely
                if (CurrentStage != null)
                {
                    CurrentStage.transform.position = currentPos;
                }
                
                if (DupeStage != null)
                {
                    DupeStage.transform.position = dupePos;
                }
            }
            yield return new WaitUntil(() => Time.timeScale > 0);
            
        }
        
        ActualLocation = TargetPosition;
        yield break;
    }
    
    private bool IsValidPosition(Vector3 pos)
    {
        return !float.IsNaN(pos.x) && !float.IsNaN(pos.y) && !float.IsNaN(pos.z) &&
               !float.IsInfinity(pos.x) && !float.IsInfinity(pos.y) && !float.IsInfinity(pos.z);
    }

    public void Initiate()
    {
        CurrentStage = GameObject.Find(Stage.StageName);
        
        if (CurrentStage == null)
        {
            Debug.LogError("CurrentStage not found: " + Stage.StageName);
            return;
        }
        
        if (_scrollRoutine != null)
        {
            StopCoroutine(_scrollRoutine);
            _scrollRoutine = null;
        }
        
        if (DupeStage != null)
        {
            Destroy(DupeStage);
        }
        
        // Wait for renderer to be ready
        StartCoroutine(InitializeWithDelay());
    }

    private IEnumerator InitializeWithDelay()
    {
        yield return new WaitUntil(() => Time.timeScale > 0);
        Renderer renderer = CurrentStage.GetComponent<Renderer>();
        
        if (renderer == null)
        {
            Debug.LogError("No Renderer found on CurrentStage!");
            yield break;
        }
        
        // Try to get BGHeight, with retries
        int retries = 0;
        BGHeight = 0;
        
        while (BGHeight <= 0 && retries < 5)
        {
            BGHeight = renderer.bounds.size.y;
            
            if (BGHeight <= 0)
            {
                retries++;
                yield return new WaitUntil(() => Time.timeScale > 0);
                 // Wait another frame
            }
        }
        
        // Final validation
        if (BGHeight <= 0 || float.IsNaN(BGHeight) || float.IsInfinity(BGHeight))
        {
            Debug.LogWarning("Could not get valid BGHeight from renderer after retries. Using default.");
            
            // Try to get from sprite if available
            SpriteRenderer spriteRenderer = CurrentStage.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteRenderer.sprite != null)
            {
                BGHeight = spriteRenderer.sprite.bounds.size.y;
                Debug.Log("Got BGHeight from SpriteRenderer: " + BGHeight);
            }
            else
            {
                BGHeight = 10f; // Fallback
                Debug.LogWarning("Using fallback BGHeight: " + BGHeight);
            }
        }
        
        // Store safe backup value
        SafeBGHeight = BGHeight;
        Debug.Log("BGHeight initialized successfully: " + BGHeight);
        
        ActualLocation = new Vector2(0, 0);
        TargetPosition = new Vector2(0, Stage.ScrollCoordinate);
        StageCloned = false;
        Stage.inStage = true;
        
        DupeStage = Instantiate(CurrentStage, new Vector3(0, BGHeight, 0), Quaternion.identity);
        DupeStage.name = Stage.StageName + "_Clone";
        StageCloned = true;
        
        StartCoroutine(StageScrolling());
    }

    public void LeaveStage()
    {   
        if (isExiting) return; 
        isExiting = true;
        StageCloned = false;
        Stage.inStage = false;
        
        // Stop all coroutines
        StopAllCoroutines();
        
        if (_scrollRoutine != null)
        {
            StopCoroutine(_scrollRoutine);
            _scrollRoutine = null;
        }
        
        if (DupeStage != null)
        {
            Destroy(DupeStage);
        }
        isExiting = false;
        SceneManager.LoadSceneAsync("StageList");
    }
    
    // Add this to handle pause/unpause events
    private void OnApplicationPause(bool pauseStatus)
    {
        // Re-validate BGHeight when returning from pause
        if (!pauseStatus && CurrentStage != null)
        {
            Renderer renderer = CurrentStage.GetComponent<Renderer>();
            if (renderer != null)
            {
                float newHeight = renderer.bounds.size.y;
                if (newHeight > 0 && !float.IsNaN(newHeight) && !float.IsInfinity(newHeight))
                {
                    BGHeight = newHeight;
                    SafeBGHeight = newHeight;
                }
            }
        }
    }
}