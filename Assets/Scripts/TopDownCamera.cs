using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public Transform target;             
    public float mouseSensitivity = 2.0f; 
    public float eyeHeight = 0.25f;        

    private float xRotation = 0f;         //Up/down look angle

    void Start()
    {
        //Lock and hide cursor
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        //Find player if not assigned
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        //mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        //looking up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        //looking left/right
        target.Rotate(Vector3.up * mouseX);

        //follow player
        transform.position = new Vector3(
            target.position.x,
            target.position.y + eyeHeight,
            target.position.z
        );

        //matches y rotation first then x
        transform.rotation = Quaternion.Euler(xRotation, target.eulerAngles.y, 0f);
    }
}