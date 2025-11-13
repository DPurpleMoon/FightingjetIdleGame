using UnityEngine;

// ScriptableObject for storing information about ascension milestones.
[CreateAssetMenu(fileName = "AscensionData", menuName = "Ascension/New Ascension Data")]
public class AscensionData : ScriptableObject
{
    public string ascensionName;      // Name of the ascension milestone
    public string description;        // Description shown in UI
    public int requiredStage;         // Stage required to unlock ascension
    public int statBoostAmount;       // Permanent stat boost given when ascended
}