using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public float bounceForce = 5f; // Force to apply on bounce
    public float pitchBounceForce = 100f; // Force to apply when bouncing off the pitch
    public float bombDamage = 50f; // Damage dealt by the bomb

    private Rigidbody rb;
    private GameManager gameManager; // Reference to the GameManager
    private PlayerHealth playerHealth; // Reference to the PlayerHealth
    private PlayerController playerController; // Reference to the PlayerController
    private BatController batController; // Reference to the BatController

    public ParticleSystem SmokeEffect;
    public ParticleSystem WaterSplashEffect;
    public ParticleSystem TraingleExplosionEffect;
    public GameObject explosionEffect; // Reference to the explosion effect for bombs

    public static float pitchSpinForceInitial = 5f; // Initial pitch spin force
    public static float pitchSpinForceMid = 10f; // Pitch spin force after 30 balls
    public static float pitchSpinForceHigh = 20f; // Pitch spin force after 35 balls

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene
        playerHealth = FindObjectOfType<PlayerHealth>(); // Find the PlayerHealth in the scene
        playerController = FindObjectOfType<PlayerController>(); // Find the PlayerController in the scene
        batController = FindObjectOfType<BatController>(); // Find the BatController in the scene

        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from the ball.");
        }
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not found in the scene.");
        }
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth is not found in the scene.");
        }
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not found in the scene.");
        }
        if (batController == null)
        {
            Debug.LogError("BatController is not found in the scene.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bat"))
        {
            HandleBatCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Pitch"))
        {
            HandlePitchCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Stumps"))
        {
            HandleStumpsCollision();
        }
    }

    void HandleBatCollision(Collision collision)
    {
        if (CompareTag("Ball"))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 bounceDirection = Vector3.Reflect(transform.forward, contact.normal);
            rb.AddForce(bounceDirection * (pitchBounceForce * 2), ForceMode.Impulse);
            rb.AddForce(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), ForceMode.Impulse);

            // Increment the score
            gameManager.AddScore(1);
        }
        else if (CompareTag("Bomb"))
        {
            Debug.Log("Bomb hit detected.");
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Explosion effect prefab is not assigned.");
            }

            // Add explosion force and damage
            rb.AddExplosionForce(pitchBounceForce, transform.position, 5f);
            playerHealth.TakeDamage(bombDamage); // Apply damage to the player

            // Apply bomb effect to player and bat controller
            if (playerController != null)
            {
                playerController.ApplyBombEffect();
            }
            if (batController != null)
            {
                batController.ApplyBombEffect();
            }

            Destroy(gameObject);
        }
        else if (CompareTag("Tomato"))
        {
            Debug.Log("Tomato hit detected.");
            if (TraingleExplosionEffect != null)
            {
                Instantiate(TraingleExplosionEffect, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("TriangleExplosionEffect prefab is not assigned.");
            }

            // Optionally, you may want to handle the behavior of the tomato hit here
            Destroy(gameObject);
        }
        else if (CompareTag("Egg"))
        {
            Debug.Log("Egg hit detected.");
            if (WaterSplashEffect != null)
            {
                Instantiate(WaterSplashEffect, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("WaterSplashEffect prefab is not assigned.");
            }

            // Optionally, you may want to handle the behavior of the egg hit here
            Destroy(gameObject);
        }
    }

    void HandlePitchCollision(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 spinDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        rb.AddTorque(spinDirection * GetCurrentPitchSpinForce(), ForceMode.Impulse);
        Vector3 bounceDirection = Vector3.Reflect(transform.forward, contact.normal);
        rb.AddForce(bounceDirection * pitchBounceForce, ForceMode.Impulse);
        rb.AddForce(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), ForceMode.Impulse);
    }

    void HandleStumpsCollision()
    {
        // Only increment the wicket count if the tag is "Ball"
        if (CompareTag("Ball"))
        {
            gameManager.AddWicket();

            // Reset values if wickets are increased
            if (batController != null)
            {
                batController.ResetValues();
            }
            if (playerController != null)
            {
                playerController.ResetValues();
            }
        }

        // Optionally, you may destroy the ball or reset its position here
        Destroy(gameObject);
    }

    float GetCurrentPitchSpinForce()
    {
        if (BallSpawner.ballCounter >= 35)
        {
            return pitchSpinForceHigh;
        }
        else if (BallSpawner.ballCounter >= 30)
        {
            return pitchSpinForceMid;
        }
        else
        {
            return pitchSpinForceInitial;
        }
    }
}
