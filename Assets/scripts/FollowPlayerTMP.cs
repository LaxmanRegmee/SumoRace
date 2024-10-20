using UnityEngine;
using TMPro;

public class FollowPlayerTMP : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public TextMeshProUGUI text;  // Reference to the TextMeshProUGUI component
    public Vector3 offset;   // Offset from the player's position

    void Update()
    {
        // Update the position of the text to follow the player with an offset
        text.transform.position = Camera.main.WorldToScreenPoint(player.position + offset);
    }
}
