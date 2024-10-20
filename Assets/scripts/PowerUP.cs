using UnityEngine;

public class PowerUP : MonoBehaviour
{
    public GameObject pickupEffect;
    public int weightChangeAmount = 10; // Amount to change the player's weight
    public int blendShapeIndex = 0; // Index of the blend shape to modify

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the PlayerWeight component from the player
            PlayerWeight playerWeight = other.GetComponent<PlayerWeight>();
            if (playerWeight != null)
            {
                // Check the tag of the power-up object
                if (gameObject.CompareTag("plus"))
                {
                    playerWeight.AdjustWeight(weightChangeAmount); // Increase weight
                    AdjustBlendShape(other, weightChangeAmount); // Increase blend shape
                }
                else if (gameObject.CompareTag("minus"))
                {
                    playerWeight.AdjustWeight(-weightChangeAmount); // Decrease weight
                    AdjustBlendShape(other, -weightChangeAmount); // Decrease blend shape
                }
            }

            // Call Pickup to handle the visual effects and destroy the power-up
            Pickup(other);
        }
    }

    void AdjustBlendShape(Collider player, int amount)
    {
        // Get the SkinnedMeshRenderer from the player
        SkinnedMeshRenderer meshRenderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
        if (meshRenderer != null)
        {
            // Get the current blend shape weight
            float currentWeight = meshRenderer.GetBlendShapeWeight(blendShapeIndex);

            // Calculate the new blend shape weight
            float newWeight = Mathf.Clamp(currentWeight + amount, 0, 100);

            // Set the new blend shape weight
            meshRenderer.SetBlendShapeWeight(blendShapeIndex, newWeight);
        }
    }

    void Pickup(Collider player)
    {
        // Instantiate the pickup effect at the power-up's position
        Instantiate(pickupEffect, transform.position, transform.rotation);

        // Destroy the power-up object immediately
        Destroy(gameObject);
    }
}
