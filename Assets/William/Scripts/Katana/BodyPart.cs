using UnityEngine;

public class BodyPartTest : MonoBehaviour
{
    [SerializeField] public Boss boss;

    public void TakeDamage(float ammount)
    {
        boss.TakeDammage(ammount);
    }
}