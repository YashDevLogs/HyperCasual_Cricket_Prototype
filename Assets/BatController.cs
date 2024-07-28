using UnityEngine;

public class BatController : MonoBehaviour
{
    public Transform batPivot;  // Reference to the bat pivot
    public float playerRotationSpeed = 6f;
    public float batSwingSpeed = 6f;
    public float batSwingAngle = 90f;
    public float playerRotationAngleLeft = 90f;  // Angle to rotate player temporarily for left click
    public float playerRotationAngleRight = 90f; // Angle to rotate player temporarily for right click

    private float originalPlayerRotationSpeed;
    private float originalBatSwingSpeed;

    private Quaternion originalPlayerRotation;
    private Quaternion originalBatRotation;
    private Quaternion targetPlayerRotation;
    private bool isSwinging = false;
    private bool rotateToTarget = false;
    private bool rotatingBack = false;
    private float rotationProgress = 0f;
    private float swingProgress = 0f;
    private Quaternion swingStartRotation;
    private Quaternion swingEndRotation;

    void Start()
    {
        // Save the original values
        originalPlayerRotationSpeed = playerRotationSpeed;
        originalBatSwingSpeed = batSwingSpeed;

        // Save the original rotation of the player and the bat pivot
        originalPlayerRotation = transform.rotation;
        originalBatRotation = batPivot.localRotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Determine the direction of the mouse click (left or right of the player)
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);

                // Decide the rotation based on where the player clicked
                if (hitPoint.x < transform.position.x)
                {
                    // Rotate left slightly for the click on the left side
                    targetPlayerRotation = originalPlayerRotation * Quaternion.Euler(0, playerRotationAngleLeft, 0);
                }
                else
                {
                    // Rotate 90 degrees to the left for the click on the right side
                    targetPlayerRotation = originalPlayerRotation * Quaternion.Euler(0, -playerRotationAngleRight, 0);
                }

                // Set up the bat swing
                SetupBatSwing();

                isSwinging = true;
                rotationProgress = 0f;
                swingProgress = 0f;

                rotateToTarget = true;
            }
        }

        if (isSwinging)
        {
            if (rotateToTarget)
            {
                RotatePlayer();
            }
            else if (rotatingBack)
            {
                ReturnToOriginalRotation();
            }
            else
            {
                SwingBat();
            }
        }
    }

    void RotatePlayer()
    {
        rotationProgress += Time.deltaTime * playerRotationSpeed;

        // Interpolate rotation to the target position
        transform.rotation = Quaternion.Lerp(originalPlayerRotation, targetPlayerRotation, rotationProgress);

        // Check if the rotation to target is complete
        if (rotationProgress >= 1f)
        {
            rotatingBack = true;
            rotationProgress = 0f;
            targetPlayerRotation = originalPlayerRotation;
        }
    }

    void ReturnToOriginalRotation()
    {
        rotationProgress += Time.deltaTime * playerRotationSpeed;

        // Interpolate rotation back to the original position
        transform.rotation = Quaternion.Lerp(transform.rotation, targetPlayerRotation, rotationProgress);

        // Check if the rotation back to the original is complete
        if (rotationProgress >= 1f)
        {
            rotatingBack = false;
            isSwinging = false; // Stop swinging when the rotation is complete
        }
    }

    void SwingBat()
    {
        swingProgress += Time.deltaTime * batSwingSpeed;

        // Interpolate the bat pivot rotation
        batPivot.localRotation = Quaternion.Lerp(swingStartRotation, swingEndRotation, swingProgress);

        // Check if the swing is complete
        if (swingProgress >= 1f)
        {
            // Reset the bat pivot to the original rotation after the swing
            batPivot.localRotation = originalBatRotation;
            isSwinging = false;
        }
    }

    void SetupBatSwing()
    {
        // Save the original rotation of the bat pivot
        originalBatRotation = batPivot.localRotation;

        // Define the start and end rotations for the bat swing
        swingStartRotation = originalBatRotation;

        float swingDirection = transform.position.x < Camera.main.ScreenToWorldPoint(Input.mousePosition).x ? 1f : -1f;

        swingEndRotation = originalBatRotation * Quaternion.Euler(0, swingDirection * batSwingAngle, swingDirection * batSwingAngle);
    }

    public void ApplyBombEffect()
    {
        // Reduce player rotation speed and bat swing speed by 50%
        playerRotationSpeed *= 0.5f;
        batSwingSpeed *= 0.5f;
    }

    public void ResetValues()
    {
        // Reset to the original values
        playerRotationSpeed = originalPlayerRotationSpeed;
        batSwingSpeed = originalBatSwingSpeed;
    }
}
