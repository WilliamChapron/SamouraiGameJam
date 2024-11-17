using UnityEngine;

public class HurtPlayer : MonoBehaviour {
    public bool hurt = true;
    [SerializeField] float dammage;
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && hurt) {
            other.gameObject.GetComponent<HealthComponent>().TakeDamage(dammage);
        }
    }
}