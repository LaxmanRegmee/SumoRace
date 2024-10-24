using UnityEngine;

public class OpponentController : MonoBehaviour
{
    [SerializeField] private Animator opponentAnimator; // Reference to the opponent's animator
    private PlayerController mainPlayerController; // Reference to the main player's script
    private Rigidbody opponentRigidbody; // Reference to the opponent's Rigidbody
     private float constantPushForce=10f; // Constant force applied when pushing
    private bool isPushing = false; // Track if the opponent is pushing
    private bool hasFallen = false; // Track if the opponent has fallen

    void Start()
    {
        if (opponentAnimator == null)
        {
            opponentAnimator = GetComponent<Animator>();
        }

        // Find the main player's PlayerController script dynamically
        mainPlayerController = FindObjectOfType<PlayerController>();

        if (mainPlayerController == null)
        {
            Debug.LogError("Main PlayerController not found!");
        }

        // Get the Rigidbody component attached to the opponent
        opponentRigidbody = GetComponent<Rigidbody>();

        if (opponentRigidbody == null)
        {
            Debug.LogError("Opponent Rigidbody not found!");
        }

        // Initially, set the opponent to idle animation
        opponentAnimator.SetBool("isIdle", true);
        opponentAnimator.SetBool("isPushing", false);
        opponentAnimator.SetBool("isWon", false);
    }
    void FixedUpdate()
    {
         if (mainPlayerController != null)
        {
            // Check if the main player has finished rotating and opponent is not already pushing
            if (mainPlayerController.HasFinishedRotating() && !isPushing)
            {
                // Switch from idle to pushing animation
                opponentAnimator.SetBool("isIdle", false);
                opponentAnimator.SetBool("isPushing", true);
                isPushing = true; // Ensure this happens only once
                
    
            }
        }
        if (isPushing && !hasFallen)
        {
            // Apply constant force to the opponent in the forward direction
            opponentRigidbody.AddForce(transform.forward * constantPushForce, ForceMode.Force);

        }
    }

   

    private void OnTriggerEnter(Collider other)
{
    // Check if opponent reached the fall trigger point
    if (other.CompareTag("FallTriggeropp") && isPushing && !hasFallen)
    {
        // Stop the pushing animation
        opponentAnimator.SetBool("isPushing", false);

        // Stop applying force to the opponent
        isPushing = false;

        // Allow the opponent to fall freely (gravity will take over)
        opponentRigidbody.constraints = RigidbodyConstraints.None;

        hasFallen = true; // Ensure this happens only once
        opponentAnimator.SetBool("isWon", false);

        // Call the TriggerPlayerVictory method in PlayerController
        if (mainPlayerController != null)
        {
            mainPlayerController.RotateInPlace();
            mainPlayerController.TriggerPlayerVictory();
        }
    }
}

}