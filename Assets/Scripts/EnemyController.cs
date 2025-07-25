using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3.5f;           //His speed
    public float rotationSpeed = 90.0f;      //How fast he turns

    public float catchDistance = 1.2f;       //How close to catch

    public GameObject playerObject;          

    //Internal variables
    private bool isChasing = true;
    private Vector3 lastKnownPlayerPos;

    void Start()
    {
        lastKnownPlayerPos = playerObject.transform.position;
    }

    void Update()
    {
        if (!isChasing || playerObject == null) return;

        //Update last known position
        lastKnownPlayerPos = playerObject.transform.position;

        //Check if close enough to catch player
        float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
        if (distanceToPlayer <= catchDistance)
        {
            CatchPlayer();
            return;
        }
        ChasePlayer();
    }

    void ChasePlayer()
    {
        //Calculate direction to player
        Vector3 directionToPlayer = lastKnownPlayerPos - transform.position;
        directionToPlayer.y = 0; 

        //Rotate toward player
        if (directionToPlayer.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            //Only moves forward if facing the right direction
            float facingAccuracy = Vector3.Dot(transform.forward, directionToPlayer.normalized);
            if (facingAccuracy > 0.5f) //Within ~60 degrees
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
    }

    //if caught show lost screen
    void CatchPlayer()
    {
        Debug.Log("Player caught!");
        isChasing = false;

        GameManager gameManager = Object.FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.ShowLoseScreen();
        }
    }
}