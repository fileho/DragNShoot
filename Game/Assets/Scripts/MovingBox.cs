using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MovingBox : MonoBehaviour
{
    // Needs dynamic rb instead of kinematic due to the custom physics simulation
    // Needs a massive mass not to be effected by the ball
    // For the same reason it has frozen rotation and X/Y position
    [SerializeField] private float speed = 1;


    [System.Serializable]
    public class OffsetData
    {
        public float offset;
        public bool horizontal;
    }


    [SerializeField] private List<OffsetData> offsets = new List<OffsetData>();

    private List<Vector3> worldPositions;
    private int currentGoal = 1;
    private int movementDirection = 1;


    private Vector3 position;
    private Vector2 velocity;

    private Rigidbody2D rb;
    private BoxCollider2D box;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();

        BuildWorldPositions();
        rb.mass = 1000000;

        NavigateLine(worldPositions[0], worldPositions[1]);
    }

    private void Update()
    {
        GoalReached();
    }

    private void GoalReached()
    {
        Vector3 goal = worldPositions[currentGoal];

        if ((transform.position - goal).magnitude > 0.1f) return;

        if (currentGoal == 0 || currentGoal == worldPositions.Count - 1)
            movementDirection *= -1;

        currentGoal += movementDirection;
        NavigateLine(goal, worldPositions[currentGoal]);
    }

    private void BuildWorldPositions()
    {
        worldPositions = new List<Vector3>(offsets.Count);

        Vector3 current = transform.position;

        foreach (var offset in offsets)
        {
            if (offset.horizontal)
                current.x += offset.offset;
            else
                current.y += offset.offset;
            worldPositions.Add(current);
        }
    }

    private void NavigateLine(Vector3 start, Vector3 end)
    {
        Vector2 dir = end - start;

        dir.Normalize();
        SetConstrains(dir);

        rb.velocity = speed * dir;
    }

    private void SetConstrains(Vector2 dir)
    {
        RigidbodyConstraints2D constraints = RigidbodyConstraints2D.FreezeRotation;

        constraints |= Mathf.Abs(dir.x) > Mathf.Abs(dir.y) ?  
            RigidbodyConstraints2D.FreezePositionY : RigidbodyConstraints2D.FreezePositionX;

        rb.constraints = constraints;
        rb.constraints = constraints;
    }


    // Draws circles on endpoints of movement when the object is selected
#if UNITY_EDITOR
    
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            BuildWorldPositions();

        Gizmos.color = Color.cyan;
        box = GetComponent<BoxCollider2D>();
        float r = Mathf.Max(box.size.x / 2, box.size.y / 2);
        
        for (var index = 0; index < worldPositions.Count; index++)
        {
            var worldPosition = worldPositions[index];
            Gizmos.DrawWireSphere(worldPosition, r);
            if (index > 0)
                Gizmos.DrawLine(worldPosition, worldPositions[index - 1]);
        }


    }
#endif

    // Caches the current physics status before the custom simulation
    public void SavePhysics()
    {
        rb.interpolation = RigidbodyInterpolation2D.None;
        position = transform.position;
        velocity = rb.velocity;
    }
    // Restores the current physics status after the custom simulation
    public void RestorePhysics()
    {
        rb.velocity = velocity;
        // Move the box according to deltaTime
        // Can be tweaked to gradually slow down time when aiming
        transform.position = position + (Vector3)rb.velocity * Time.deltaTime;
    }

    // Switches interpolation back to interpolate with standard physics
    // Has to be none for a custom simulation
    public void StandardPhysics()
    {
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }
}
