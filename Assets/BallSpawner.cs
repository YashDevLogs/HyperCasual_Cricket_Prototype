using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab; // The ball prefab (standard delivery)
    public List<GameObject> otherPrefabs; // List of other delivery prefabs (e.g., eggs, bombs)
    public List<Transform> spawnPoints; // List of spawn points
    public float throwForceInitial = 10f; // Initial throw force
    public float throwForceMid = 15f; // Throw force after 20 balls
    public float throwForceHigh = 20f; // Throw force after 30 balls
    public float spawnInterval = 2f; // Interval to spawn a new ball

    public static int ballCounter = 0; // Counter to track balls spawned
    public float ballProbability = 0.7f;

    private float ballLifetime = 3f; // Lifetime of each ball

    void Start()
    {
        ballCounter = 0; // Initialize counter
        // Start the ball spawning coroutine
        StartCoroutine(SpawnBalls());
    }

    IEnumerator SpawnBalls()
    {
        while (true)
        {
            // Spawn a ball
            SpawnBall();

            // Wait for the specified interval before spawning the next ball
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnBall()
    {
        // Check if there are spawn points
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }

        // Select a random spawn point from the list
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Decide which prefab to use based on probability
        GameObject selectedPrefab = (Random.value < ballProbability) ? ballPrefab : GetRandomOtherPrefab();

        // Instantiate the selected prefab at the chosen spawn point
        GameObject ball = Instantiate(selectedPrefab, randomSpawnPoint.position, Quaternion.identity);

        // Get the Rigidbody component of the ball
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Determine throw force based on ball counter
            float throwForce = throwForceInitial;
            if (ballCounter >= 35)
            {
                throwForce = throwForceHigh;
            }
            else if (ballCounter >= 20)
            {
                throwForce = throwForceMid;
            }

            // Apply force to the ball to throw it towards the bat
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody not found on " + selectedPrefab.name);
        }

        // Increment the ball counter
        ballCounter++;

        // Destroy the ball after a certain lifetime
        Destroy(ball, ballLifetime);
    }

    GameObject GetRandomOtherPrefab()
    {
        // Check if there are other prefabs available
        if (otherPrefabs.Count == 0)
        {
            Debug.LogWarning("No other delivery prefabs assigned.");
            return ballPrefab; // Fallback to ball prefab if no other prefabs are available
        }

        // Select a random prefab from the list of other prefabs
        return otherPrefabs[Random.Range(0, otherPrefabs.Count)];
    }

    public void ResetBallCounter()
    {
        ballCounter = 0; // Reset ball counter
    }
}
