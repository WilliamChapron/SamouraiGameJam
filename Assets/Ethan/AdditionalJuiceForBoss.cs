using UnityEngine;

public class AdditionalJuiceForBoss : MonoBehaviour
{
    [SerializeField] ParticleSystem meteor;
    [SerializeField] ParticleSystem fire;

    public void MeteorStart() {
        meteor.Play();
    }

    public void FireStart()
    {
        fire.Play();
    }
}
