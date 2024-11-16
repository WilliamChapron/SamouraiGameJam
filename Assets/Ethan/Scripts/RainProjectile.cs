using UnityEngine;

public class RainProjectile : MonoBehaviour {
    [SerializeField] GameObject EXXXPPLLLOOTTIOONNN;
    private void OnTriggerEnter(Collider other) {
        GameObject explotion = Instantiate(EXXXPPLLLOOTTIOONNN);
        explotion.transform.position = transform.position;
        Destroy(gameObject);
    }
}