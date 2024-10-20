using UnityEngine;
using TMPro;

public class LadderStepIterator : MonoBehaviour
{
    public GameObject ladderStepPrefab;
    public Transform ladderParent;
    public Transform spawnPoint;
    public float stepSpacing = -2f;
    public GameObject ringPrefab; // Prefab reference to the ring
    public float ringRiseSpeed = 2f; // Speed at which the ring rises
    public int stepsBeforeLastToShowRing = 100; // Number of steps before the last step to start the ring rising
    public Transform playerTransform; // Reference to the player

    private PlayerWeight playerWeight;
    private GameObject lastLadderStep;
    private GameObject ring;
    private bool ringIsRising = false;
    private Vector3 ringStartPosition;
    private Vector3 ringTargetPosition;
    private bool ringTriggered = false; // To prevent ring from rising too early

    void Start()
    {
        playerWeight = FindObjectOfType<PlayerWeight>();

        if (playerWeight == null)
        {
            Debug.LogError("PlayerWeight script not found on the player.");
            return;
        }

        // Subscribe to the weight change event
        playerWeight.OnWeightChanged.AddListener(UpdateLadderSteps);

        // Generate initial ladder steps
        UpdateLadderSteps(playerWeight.GetCurrentWeight());
    }

    void Update()
    {
        if (ringIsRising && ring != null)
        {
            // Smoothly move the ring up from the start position to the target position
            ring.transform.position = Vector3.Lerp(ring.transform.position, ringTargetPosition, ringRiseSpeed * Time.deltaTime);

            // Stop the rising when the ring has reached the target height
            if (Vector3.Distance(ring.transform.position, ringTargetPosition) < 0.1f)
            {
                ringIsRising = false;
            }
        }

        // Check if the player is close enough to trigger the ring rising
        if (!ringTriggered && IsPlayerNearLastSteps())
        {
            TriggerRingRise();
        }
    }

    void UpdateLadderSteps(int weight)
    {
        // Clear existing ladder steps
        foreach (Transform child in ladderParent)
        {
            Destroy(child.gameObject);
        }

        Vector3 currentPosition = spawnPoint.position;

        // Generate ladder steps based on the player's weight
        for (int i = 0; i < weight; i++)
        {
            GameObject newStep = Instantiate(ladderStepPrefab, currentPosition, Quaternion.identity, ladderParent);

            TMP_Text stepText = newStep.GetComponentInChildren<TMP_Text>();
            if (stepText != null)
            {
                stepText.text = (i + 1).ToString() + " lbs";
            }

            lastLadderStep = newStep; // Update the last step

            currentPosition += new Vector3(0, 0, stepSpacing);
        }

        // Position the ring at the last step
        Vector3 ringPosition = currentPosition - new Vector3(0, 1, -2);

        // Set the ring's start position below the last step
        ringStartPosition = ringPosition - new Vector3(0, 3, -1); // 5 units below

        // Set the target position (the actual position of the ring)
        ringTargetPosition = ringPosition;

        // Instantiate the ring in its start position at the final ladder step
        if (ring != null)
        {
            Destroy(ring); // Ensure only one ring exists
        }
        ring = Instantiate(ringPrefab, ringStartPosition, Quaternion.identity, ladderParent);

        ringTriggered = false; // Reset the ring trigger
    }

    bool IsPlayerNearLastSteps()
    {
        // Check the player's distance to the last ladder step
        if (lastLadderStep == null || playerTransform == null)
        {
            return false;
        }

        float distanceToLastStep = Vector3.Distance(playerTransform.position, lastLadderStep.transform.position);

        // Trigger when the player is within a certain number of steps (based on stepSpacing)
        return distanceToLastStep <= (stepsBeforeLastToShowRing * stepSpacing);
    }

    void TriggerRingRise()
    {
        if (ring != null)
        {
            ringTriggered = true;
            ringIsRising = true;
        }
    }

    // This method will be called when the player collides with a specific ladder step
    public void OnPlayerCollideWithMatchingStep(int stepWeight)
    {
        if (stepWeight == playerWeight.GetCurrentWeight())
        {
            Debug.Log("Player collided with the matching ladder step: " + stepWeight);
            // Logic to handle what happens when the player collides with the matching step can go here
        }
    }
}
