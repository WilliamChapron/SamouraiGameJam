using UnityEngine;

public class RainProjectile : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
    }
}