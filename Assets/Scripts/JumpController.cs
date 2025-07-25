using UnityEngine;
using System.Collections;

public class JumpController : MonoBehaviour
{
    public float jumpHeight = 15f;      //Max height of jump
    public float jumpDuration = 2f;     //jump duration
    public KeyCode jumpKey = KeyCode.J; //Key for use
    public float jumpMoveSpeed = 3f;    //How fast to move while jumping

    private PlayerInventory inventory;
    private bool isJumping = false;
    private CharacterController controller;
    private Transform cameraTransform;

    void Start()
    {
        inventory = GetComponent<PlayerInventory>();
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (!isJumping && Input.GetKeyDown(jumpKey))
        {
            if (inventory != null && inventory.UseItem("Jump"))
            {
                StartCoroutine(Jump());
            }
            else
            {
                Debug.Log("No Jump Boost in inventory!");
            }
        }
    }

    IEnumerator Jump()
    {
        if (isJumping) yield break;

        isJumping = true;

        // Disable controller for direct transform movement
        if (controller != null)
            controller.enabled = false;

        //Starting height
        float startY = 1.0f;

        //Jump
        float startTime = Time.time;

        while (Time.time < startTime + jumpDuration)
        {
            //Calculate time through jump
            float progress = (Time.time - startTime) / jumpDuration;

            //Calculate height using sine wave
            float height = startY + jumpHeight * Mathf.Sin(progress * Mathf.PI);

            //horizontal movement during jump
            float h = 0;
            float v = 0;

            //Check keys for horizontal movement
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) v = 1;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) v = -1;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) h = -1;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h = 1;

            if (h != 0 || v != 0)
            {
                Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
                Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

                Vector3 moveDirection = forward * v + right * h;
                if (moveDirection.magnitude > 1f)
                    moveDirection.Normalize();

                //horizontal movement
                Vector3 movement = moveDirection * jumpMoveSpeed * Time.deltaTime;
                transform.position += new Vector3(movement.x, 0, movement.z);
            }

            //vertical position
            Vector3 newPos = transform.position;
            newPos.y = height;
            transform.position = newPos;

            yield return null;
        }

        //Always land at y 1
        Vector3 landingPos = transform.position;
        landingPos.y = 1.0f;
        transform.position = landingPos;

        //enable controller after landing
        if (controller != null)
            controller.enabled = true;

        isJumping = false;
    }
}