using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 1.5f, -5f);
    public Vector3 offsetRotation = new Vector3(10f, 0f, 0f); // Rotation en degrés (par exemple 10° vers le bas)

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        rotationY += Input.GetAxis("Mouse X") * rotationSpeed;
        rotationX -= Input.GetAxis("Mouse Y") * rotationSpeed;


        rotationX = Mathf.Clamp(rotationX, -60f, 60f);

        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        Quaternion rotationOffset = Quaternion.Euler(offsetRotation);

        transform.position = player.position + (rotation * rotationOffset * offset);

        transform.LookAt(player);
    }
}
