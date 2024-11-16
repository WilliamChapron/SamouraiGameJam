using UnityEngine;

public class CC : MonoBehaviour {

    [Header("")]
    [SerializeField][Range(0, 10)] float speed = 8f;

    // Privates
    CharacterController character;

    void Start() {
        character = GetComponent<CharacterController>();
    }

    void Update() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float MouseX = Input.GetAxis("Mouse X");

        transform.Rotate(0, MouseX, 0);

        Vector3 movements = transform.forward * vertical + transform.right * horizontal;

        character.Move((movements * speed) * Time.deltaTime);
    }
}