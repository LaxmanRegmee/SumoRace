using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PlayerWeight : MonoBehaviour
{
    public int playerWeight = 0;  // Initial weight of the player
    public TMP_Text weightText;   // UI element to display the player's weight
    public int minWeight = 0;     // Minimum weight the player can have

    public UnityEvent<int> OnWeightChanged; // Event to notify when weight changes

    void Start()
    {
        UpdateWeightText();
    }

    public void AdjustWeight(int amount)
    {
        // Adjust the player's weight
        playerWeight += amount;

        // Ensure the player's weight doesn't drop below the minimum weight
        if (playerWeight < minWeight)
        {
            playerWeight = minWeight;
        }

        // Update the weight display
        UpdateWeightText();

        // Notify listeners about the weight change
        if (OnWeightChanged != null)
        {
            OnWeightChanged.Invoke(playerWeight);
        }
    }

    public void UpdateWeightText()
    {
        weightText.text = playerWeight.ToString() + " lbs";
    }

    public int GetCurrentWeight()
    {
        return playerWeight;
    }
}
