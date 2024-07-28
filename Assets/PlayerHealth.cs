using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public GameObject gameOverUI;
    public GameManager gameManager; // Reference to the GameManager


    void Start()
    {
        currentHealth = maxHealth;
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not found in the scene.");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
/*        SmokeEffect.gameObject.SetActive(true);
        SmokeEffect.Play();
*/
        if (currentHealth <= 0)
        {
            gameManager.AddWicket();
/*            SmokeEffect.gameObject.SetActive(false);
*/
            currentHealth = maxHealth; // Reset health for next life
        }
    }

}
