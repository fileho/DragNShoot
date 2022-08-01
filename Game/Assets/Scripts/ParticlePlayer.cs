using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule emission;

    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        emission = ps.emission;
    }


    public void PlayParticles(Vector3 worldPos, Vector2 dir, float speed)
    {
        dir.Normalize();

        transform.position = worldPos;
        transform.up = dir;


        ParticleSystem.Burst burst = new ParticleSystem.Burst(0, (int)speed);

        emission.SetBurst(0, burst);


        ps.Play();
    }
}
