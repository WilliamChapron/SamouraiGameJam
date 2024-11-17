using UnityEngine;
using UnityEngine.Events;

public class Executor : MonoBehaviour
{
    [SerializeField] UnityEvent OnEnter;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnEnter.Invoke();
        }
        
    }
}