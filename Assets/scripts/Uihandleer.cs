using UnityEngine;
using UnityEngine.UI; // Required if you're using UI components like Text or Image

public class UITapHandler : MonoBehaviour
{
    [SerializeField] private GameObject uiElement; // Reference to the UI element you want to activate
    [SerializeField] private Transform player; // Reference to the player object
    [SerializeField] private float rotationThreshold = 90f; // The rotation angle to trigger the UI activation

    private Vector3 lastRotation; // Last known rotation of the player

    void Start()
    {
        if (player != null)
        {
            lastRotation = player.eulerAngles;
        }
    }

    void Update()
    {
        // Check for a screen tap (touch or mouse click)
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            HideUIElement();
        }

        // Check for rotation
        if (player != null)
        {
            Vector3 currentRotation = player.eulerAngles;
            if (Vector3.Angle(lastRotation, currentRotation) > rotationThreshold)
            {
                ActivateUIElement();
                lastRotation = currentRotation; // Update the last rotation
            }
        }
    }

    private void ActivateUIElement()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(true); // Activate the UI element
        }
    }

    private void HideUIElement()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(false); // Hide the UI element
        }
    }
}
