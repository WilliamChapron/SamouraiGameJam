using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 1.5f, -5f);

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


        rotationX = Mathf.Clamp(rotationX, -30f, 60f);


        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);


        transform.position = player.position + rotation * offset;


        transform.LookAt(player);
    }
}
