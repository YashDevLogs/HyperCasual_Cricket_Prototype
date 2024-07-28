using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the player moves
    private float originalMoveSpeed;

    void Start()
    {
        // Save the original move speed
        originalMoveSpeed = moveSpeed;
    }

    void Update()
    {
        // Get input for horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate movement
        Vector3 movement = new Vector3(horizontalInput, 0, 0) * moveSpeed * Time.deltaTime;

        // Apply movement to the player's position
        transform.Translate(movement, Space.World);
    }

    public void ApplyBombEffect()
    {
        // Reduce movement speed by 50%
        moveSpeed *= 0.5f;
    }

    public void ResetValues()
    {
        // Reset to the original value
        moveSpeed = originalMoveSpeed;
    }
}
