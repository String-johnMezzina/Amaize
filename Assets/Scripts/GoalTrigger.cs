using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private GameManager gameManager;
    private bool triggered = false;

    void Start()
    {
        gameManager = Object.FindAnyObjectByType<GameManager>();

        gameObject.tag = "Goal";

    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        Debug.Log("Goal triggered by: " + other.name + " with tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("GOAL REACHED by player!");

            if (gameManager != null)
            {
                gameManager.ShowWinScreen();
            }
            else
            {
                Debug.LogError("GameManager not found!");
            }
        }
    }
}