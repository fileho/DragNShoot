using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ShootDrawer : MonoBehaviour
{
    [Tooltip("Shape we want to draw")]
    [SerializeField] private GameObject prefab;
    [Tooltip("Number of those shapes")]
    [SerializeField] private int count;
    [Tooltip("Total duration simulated between each point")]
    [Range(0, 1f)]
    [SerializeField] private float simulationTime;

    private readonly List<GameObject> instances = new List<GameObject>();
    private Rigidbody2D rb;

    private MovingBox[] movingBoxes;
    

    private void Start()
    {
        var parent = new GameObject("Aiming dots").transform;

        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(prefab) as GameObject;
            instances.Add(go);
            go.transform.SetParent(parent);
        }
        rb = GetComponent<Rigidbody2D>();
        Physics2D.autoSyncTransforms = true;

        movingBoxes = FindObjectsOfType<MovingBox>();

        // Set last point as follow for aiming camera
        Aiming.instance.SetAimingTarget(instances[instances.Count - 1].transform);

        Clear();
    }

    public void DrawShot(Vector2 direction)
    {        
        // Switch to custom physics simulation
        rb.interpolation = RigidbodyInterpolation2D.None;
        StopMovingBoxes();
        Physics2D.simulationMode = SimulationMode2D.Script;

        // Update physics (flush pipeline)
        Physics2D.Simulate(0);

        // Reset data to default and cache current position
        Vector3 startPos = transform.position;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.AddForce(direction);

        // Custom simulation
        for (int i = 0; i < count; i++)
        {
            instances[i].SetActive(true);

            // The simulation itself
            // Uses multiple smaller step simulation for better precision
            Physics2D.Simulate(simulationTime * 0.1f);
            Physics2D.Simulate(simulationTime * 0.1f);
            Physics2D.Simulate(simulationTime * 0.1f);
            // Places one aiming dot on the current position during the simulation
            instances[i].transform.position = transform.position;
        }
        // Restore data
        rb.velocity = Vector2.zero;
        transform.position = startPos;
        RestoreMovingBoxes();
        // Update physics (flush pipeline again)
        Physics2D.Simulate(0f);
    }

    private void StopMovingBoxes()
    {
        foreach (var mb in movingBoxes) mb.SavePhysics();
    }

    private void RestoreMovingBoxes()
    {
        foreach (var mb in movingBoxes) mb.RestorePhysics();
    }

    public void SwitchToFixedDelta()
    {
        if (Physics2D.simulationMode == SimulationMode2D.FixedUpdate)
            return;

        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        foreach (var mb in movingBoxes) mb.StandardPhysics();
    }

    public void Clear()
    {
        foreach (var i in instances)
        {
            i.SetActive(false);
        }
    }
}
