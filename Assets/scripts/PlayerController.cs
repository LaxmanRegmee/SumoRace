    using UnityEngine;
    using System.Collections;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float swipeSpeed;
        [SerializeField] private Joystick joystick;
        [SerializeField] private Animator animator;

        [SerializeField] private float speedMultiplier = 1.5f;
        [SerializeField] private float slowDownMultiplier = 0.5f;
        [SerializeField] private float zCoordinate = 50f;
        [SerializeField] private float slowDownRange = 5f;
        [SerializeField] private float pushForce = 50f;
        [SerializeField] private Transform resetPosition;
        public GameObject victoryPanel;
        public GameObject losingPanel;
        public GameObject tappanel;
        public GameObject MainCam;
        [SerializeField] private GameObject opps; // Opponent reference

        private bool isControlled = false;
        private bool speedIncreased = false;
        private bool isPlayerStopped = false;
        private GameObject ring;
        private Transform ringTarget;
        private bool isMovingToRingTarget = false;
        private bool hasFinishedRotating = false;
        private bool opponentFell = false;
        private bool hasFallen = false; // Track if the player has fallen

        void Start()
        {
            rb = GetComponent<Rigidbody>();

            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            animator.SetBool("isWalking", false);
            animator.SetBool("isPushing", false);
            animator.SetBool("isWon", false);

        }

        void Update()
        {
            if (Input.touchCount > 0 && !isControlled)
            {
                isControlled = true;
                animator.SetBool("isWalking", true);
            }

            if (isControlled && !speedIncreased && transform.position.z > zCoordinate)
            {
                Vector3 newPosition = transform.position;
                newPosition.x = 0;
                transform.position = newPosition;

                moveSpeed *= speedMultiplier;
                speedIncreased = true;
                joystick.gameObject.SetActive(false);
            }

            if (ring != null)
            {
                float distanceToRing = Vector3.Distance(transform.position, ring.transform.position);
                if (distanceToRing < slowDownRange)
                {
                    moveSpeed *= slowDownMultiplier;
                }
                else
                {
                    moveSpeed /= slowDownMultiplier;
                }
            }


            if (hasFinishedRotating && Input.touchCount > 0)
            {
                AddPushForce();

            }


        }

        void FixedUpdate()
        {
            if (isControlled && !isPlayerStopped && !isMovingToRingTarget && !hasFallen)
            {
                rb.velocity = new Vector3(joystick.Horizontal * swipeSpeed, 0, moveSpeed * Time.deltaTime);
            }
            else if (isPlayerStopped || isMovingToRingTarget || hasFallen || opponentFell)
            {
                rb.velocity = Vector3.zero;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("RingPrefab"))
            {
                ringTarget = other.transform.Find("TargetPosition");

                if (ringTarget != null)
                {
                    StartCoroutine(MoveToRingTarget(ringTarget.position));
                }

                // CameraController cameraController = FindObjectOfType<CameraController>();
                CameraController cc = MainCam.GetComponent<CameraController>();
                Debug.Log("istopped");
                cc.StopFollowingPlayer();
            }

            if (other.CompareTag("FallTriggerplay"))
            {
                // If player collides with the fall trigger
                if (!hasFallen)
                {
                    Debug.Log("insidefall");

                    Fall(); // Call the fall function when player collides with fall trigger
                }
            }
        }


        private void Fall()
        {
            hasFallen = true;

            // Play falling animation
            animator.SetTrigger("Fall");
            animator.SetBool("isPushing",false);

            // Remove Rigidbody constraints to allow the player to fall freely
            rb.constraints = RigidbodyConstraints.None;


            // Clear any movement forces
            rb.velocity = Vector3.zero;
            MainCam.transform.position = new Vector3(-0.74f,1.4481f,73.7f);
            MainCam.transform.rotation = Quaternion.Euler(15,0,0);

            // // Switch camera focus to opponent
            // CameraController cc = MainCam.GetComponent<CameraController>();
            // Debug.Log("fallistopped");
            // if (cc != null)
            // {
            //     Debug.Log("iamnotnull");
            //     cc.FocusOnOpponent(opponent.transform);
            // }
            // else
            // {
            //     Debug.Log("ccisnull");
            // }

            // Optionally, display losing UI if needed
            losingPanel.SetActive(true);
        }


        private IEnumerator MoveToRingTarget(Vector3 targetPosition)
        {
            float moveDuration = 1.0f;
            float elapsedTime = 0f;
            Vector3 initialPosition = transform.position;
            isMovingToRingTarget = true;

            while (elapsedTime < moveDuration)
            {
                transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;

            animator.SetBool("isWalking", false);


            Quaternion initialRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0, 90, 0);
            float rotationDuration = 1.0f;
            elapsedTime = 0f;

            while (elapsedTime < rotationDuration)
            {
                transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / rotationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;
            hasFinishedRotating = true;
            StopPlayer();
            tappanel.SetActive(true);
            animator.SetBool("isPushing", true);
            
            // opps=opponent.GetComponent<Animator>(){
            //      opps.SetBool("isPushing",true);
            // isMovingToRingTarget = false;
            // }
           
        }

        public void StopPlayer()
        {
            isPlayerStopped = true;
            moveSpeed = 0;
        }

        private void AddPushForce()
        {
            if (!opponentFell && !hasFallen)
            {
                rb.AddForce(transform.forward * pushForce, ForceMode.Impulse);
            }
            if (opponentFell)
            {
                rb.AddForce(transform.forward * 0, ForceMode.Impulse);
                tappanel.SetActive(false);
            }

        }

        public void MoveToRingTargetPosition()
        {
            StartCoroutine(RotateInPlace());
        }

        public bool HasFinishedRotating()
        {
            return hasFinishedRotating;
        }

       public IEnumerator RotateInPlace()
{
    Quaternion initialRotation = transform.rotation;
    Quaternion targetRotation = Quaternion.Euler(0, 180, 0);
    float elapsedTime = 0f;
    float rotationDuration = 1.0f;

    rb.velocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;

    while (elapsedTime < rotationDuration)
    {
        transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / rotationDuration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    transform.rotation = targetRotation;

    StopPlayerPush();
}


        public void StopPlayerPush()
        {

            rb.velocity = Vector3.zero;

        }
  public void TriggerPlayerVictory()
{
    // Trigger the player's victory animation
    animator.SetBool("isPushing",false);
    animator.SetBool("isWon", true);
    victoryPanel.SetActive(true);
    tappanel.SetActive(false);
    StopPlayer();
}
}