using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Reference to the player
    [SerializeField] private float zCoordinateThreshold = 31.5f; // The z-coordinate threshold
    [SerializeField] private Vector3 rotationOffset = new Vector3(0, 30, 0); // The rotation offset at the threshold
    [SerializeField] private Vector3 positionOffset = new Vector3(0, 5, -10); // The position offset relative to the player
    [SerializeField] private Vector3 additionalPositionOffset = new Vector3(0, 5, -5); // Additional position offset when the threshold is reached
    [SerializeField] private Vector3 stopPositionOffset = new Vector3(0, 10, -15); // New offset when camera stops
    [SerializeField] private Vector3 stopRotationOffset = new Vector3(15, 30, 0); // New rotation when the camera stops
    [SerializeField] private float transitionSpeed = 2f; // Speed of the smooth transition

    private Vector3 initialPosition; // To store the initial position of the camera
    private Quaternion initialRotation; // To store the initial rotation of the camera
    private Quaternion targetRotation; // Target rotation for smooth transition
    private bool stopFollowingPlayer = false; // Flag to indicate whether to stop following the player
    private bool isFocusingOnOpponent = false; // Flag to indicate if camera is focusing on the opponent
    public Transform opponentTransform; // Reference to the opponent

    void FixedUpdate() 
    {
        // If the camera is focusing on the opponent
        if (isFocusingOnOpponent && opponentTransform != null)
        {
            // Continuously update to the opponent's position and rotation
            Vector3 opponentTargetPosition = opponentTransform.position + stopPositionOffset; // Focus on opponent
            Quaternion opponentTargetRotation = Quaternion.Euler(stopRotationOffset);

            // Smoothly transition to the opponent's position and rotation
            transform.position = Vector3.Lerp(transform.position, opponentTargetPosition, transitionSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, opponentTargetRotation, transitionSpeed * Time.deltaTime);

            return; // Exit early since we're focusing on the opponent
        }

        // If the camera should stop following the player
        if (stopFollowingPlayer)
        {
            // Transition to stop offsets
            Vector3 stopTargetPosition = playerTransform.position + stopPositionOffset;
            Quaternion stopTargetRotation = Quaternion.Euler(stopRotationOffset);

            // Smoothly transition the position
            transform.position = Vector3.Lerp(transform.position, stopTargetPosition, transitionSpeed * Time.deltaTime);

            // Smoothly transition the rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, stopTargetRotation, transitionSpeed * Time.deltaTime);

            return;
        }

        // Calculate the camera's target position based on player position and offsets
        Vector3 playerTargetPosition = playerTransform.position + positionOffset;

        // Check if the player's z-coordinate exceeds the threshold
        if (playerTransform.position.z >= zCoordinateThreshold)
        {
            // Apply additional position offset and update target rotation
            playerTargetPosition += additionalPositionOffset;
            targetRotation = Quaternion.Euler(rotationOffset);
        }
        else
        {
            // Reset to initial position and rotation if below threshold
            playerTargetPosition = playerTransform.position + positionOffset;
            targetRotation = initialRotation;
        }

        // Smoothly transition the position
        transform.position = Vector3.Lerp(transform.position, playerTargetPosition, transitionSpeed * Time.deltaTime);

        // Smoothly transition the rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, transitionSpeed * Time.deltaTime);
    }

    // This method will be called when the player collides with the ring
    public void StopFollowingPlayer()
    {
        stopFollowingPlayer = true;
    }
    
    // Focus on the opponent when the player falls
     public void FocusOnOpponent(Transform opponentTransform)
     {
         Debug.Log("isfocusonoppest");
         isFocusingOnOpponent = true;
         this.opponentTransform = opponentTransform;
       stopFollowingPlayer = false; // Ensure the camera no longer follows the player
    }
}
