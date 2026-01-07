using UnityEngine;

[CreateAssetMenu(fileName = "AscensionItem", menuName = "Shop/Special Item")]
public class SpecialItemData : ScriptableObject
{
    public string itemName = "Ascension";
    [TextArea] public string description = "Reset ALL progress to gain permanent +10% Damage & Income.";
    public Sprite icon;

    [Header("Cost Settings")]
    public double baseCost = 10000;
    public float costMultiplier = 5.0f; // Gets expensive fast!

    // Calculates price based on how many times you have ascended
    public double GetCost()
    {
        // Safety check to prevent errors in Editor
        if (AscensionManager.Instance == null) return baseCost;

        int count = AscensionManager.Instance.ascensionTokens;
        return baseCost * System.Math.Pow(costMultiplier, count);
    }
}