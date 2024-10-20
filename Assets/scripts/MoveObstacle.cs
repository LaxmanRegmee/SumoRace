using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    public float speed = 5f;       // Speed of the movement
    public float distance = 10f;   // Distance to move from the starting point

    private Vector3 startPosition;
    private int direction = 1;     // Direction of movement (1 = right, -1 = left)

    void Start()
    {
        // Record the starting position
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new position
        float newX = transform.position.x + direction * speed * Time.deltaTime;

        // If the obstacle has moved the desired distance, reverse direction
        if (Mathf.Abs(newX - startPosition.x) > distance)
        {
            direction *= -1;
        }

        // Apply the new position
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}

