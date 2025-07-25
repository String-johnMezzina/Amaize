using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private CharacterController controller;
    private Transform cameraTransform;

    void Start()
    {
        //add CharacterController
        controller = GetComponent<CharacterController>();
        if (controller == null)
            controller = gameObject.AddComponent<CharacterController>();

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            cameraTransform = mainCamera.transform;
        }
    }

    void Update()
    {
        //Skip if controller is disabled (during jumps)
        if (controller == null || !controller.enabled)
            return;

        //input values
        float h = 0;
        float v = 0;

        //Check keys 
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) v = 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) v = -1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) h = -1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h = 1;

        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

        //Calculate movement direction
        Vector3 moveDirection = forward * v + right * h;
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        //Apply movement using CharacterController
        if (moveDirection.magnitude > 0)
        {
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        CheckForGoal();
    }

    void CheckForGoal()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.4f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Goal") || hitCollider.name.Contains("MazeGoal"))
            {
                GameManager gameManager = Object.FindFirstObjectByType<GameManager>();
                if (gameManager != null)
                {
                    gameManager.ShowWinScreen();
                    break;
                }
            }
        }
    }
}