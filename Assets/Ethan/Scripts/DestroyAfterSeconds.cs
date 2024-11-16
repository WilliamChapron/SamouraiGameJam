using System.Collections;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour {
    [SerializeField] float seconds = 1.2f;
    void Start() {
        StartCoroutine(DetroyAfterSecs());
    }
    IEnumerator DetroyAfterSecs() {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}