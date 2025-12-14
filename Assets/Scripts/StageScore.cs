using UnityEngine;

public class StageScore : MonoBehaviour
{
    public static StageScore Instance { get; private set; } 
    
    // An event that other scripts can subscribe to for UI updates (BEST PRACTICE)
    public event Action<int> OnScoreChanged;

    private int currentScore = 0;

    // Public property to safely read the score.
    public int CurrentScore
    {
        get { return currentScore; }
    }

    private void Awake()
    {
        // Implement the Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void AddPoints(int points)
    {
        if (points > 0)
        {
            currentScore += points;
            
            // Log the update to the Unity Console
            Debug.Log($"Score: {currentScore}");
            
            // Notify any subscribed UI/logic elements
            OnScoreChanged?.Invoke(currentScore);
        }
    }
}
