using UnityEngine;

public class HurtPlayer : MonoBehaviour {

    [SerializeField] float dammage;
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<HealthComponent>().TakeDamage(dammage);
        }
    }
}