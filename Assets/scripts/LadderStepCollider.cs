using UnityEngine;

public class LadderStepCollider : MonoBehaviour
{
    private LadderStepIterator ladderStepIterator;
    private int stepWeight;

    // Set up the reference to LadderStepIterator and step weight
    public void Setup(LadderStepIterator ladderStepIterator, int stepWeight)
    {
        this.ladderStepIterator = ladderStepIterator;
        this.stepWeight = stepWeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ensure this is the player and not the ring itself
        if (other.CompareTag("Player")) // Ensure player has the correct tag
        {
            ladderStepIterator.OnPlayerCollideWithMatchingStep(stepWeight);
        }
    }
}
