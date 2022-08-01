using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ParticleController : MonoBehaviour
{
    [Tooltip("Amount of possible particles effects playing at the same time")]
    [SerializeField] private int poolSize = 5;

    public static ParticleController instance;

    private int index;

    private ParticlePlayer[] pool;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameObject child = transform.GetChild(0).gameObject;

        for (int i = 0; i < poolSize - 1; i++)
            Instantiate(child, Vector3.zero, Quaternion.identity, transform);


        pool = GetComponentsInChildren<ParticlePlayer>();
    }


    // Uses a pool system to avoid allocations
    public void PlayParticles(Vector3 worldPos, Vector2 dir, float speed)
    {
        pool[index].PlayParticles(worldPos, dir, speed);

        ++index;
        index %= pool.Length;
    }


}
