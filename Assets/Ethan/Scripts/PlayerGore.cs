using UnityEngine;

public class PlayerGore : MonoBehaviour {
    [SerializeField] float maxDist;
    [SerializeField] GameObject particle;

    void Update() {
        RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, maxDist))
                {
                    if (hit.collider.CompareTag("Boss"))
                    {
                        GameObject blood = Instantiate(particle) as GameObject;
                        blood.gameObject.transform.position = hit.point;
                        blood.transform.rotation = Quaternion.LookRotation(-hit.normal);
                    }

                }

    }
}