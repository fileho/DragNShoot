using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float maxSpeed;

    private Rigidbody2D rb;
    private ShootDrawer shootDrawer;
    private SpriteRenderer spriteRenderer;
    private Vector2 startPos;
    private const float minSpeed = 0.15f;

    private bool initialized = false;
    private bool cancel = false;

    private int interactable = 0;


    public static Ball instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shootDrawer = GetComponent<ShootDrawer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        // Has to be called in update not in fixed Update to work in both simulation modes
        HandleInput();
    }

    public void SetInteractable(int val)
    {
        interactable += val;
    }

    public bool IsActive()
    {
        return interactable == 0 && Physics2D.simulationMode == SimulationMode2D.Script;
    }

    public void StopAiming()
    {
        shootDrawer.SwitchToFixedDelta();
        Aiming.instance.StopAiming();

        initialized = false;
        cancel = true;
        shootDrawer.Clear();
    }


    private void HandleInput()
    {
        if (BlockedInput()) return;

        if (HandleCancel()) return;

        // totally stop the ball if it is moving very slowly
        // rb.velocity = Vector2.zero;

        spriteRenderer.color = Color.white;

        // Aiming
        if (Input.GetMouseButton(0))
        {
            if (!initialized)
            {
                initialized = true;
                Aiming.instance.StartAiming();
                startPos = GetCurrentPosition();
                return;
            }

            shootDrawer.DrawShot(CalculateVector(startPos - (Vector2)Input.mousePosition));
            return;
        }

        // Shoot
        if (Input.GetMouseButtonUp(0))
        {
            // switch back to normal fixed update physics evaluation
            shootDrawer.SwitchToFixedDelta();
            Aiming.instance.StopAiming();

            initialized = false;

            MoveBall(CalculateVector(startPos - (Vector2)Input.mousePosition));
            shootDrawer.Clear();
        }
    }

    private bool BlockedInput()
    {
        // input is blocked (settings)
        if (rb.velocity.magnitude > minSpeed || interactable > 0)
        {
            float col = 0.85f;
            spriteRenderer.color = new Color(col, col, col);

            initialized = false;
            shootDrawer.Clear();
            shootDrawer.SwitchToFixedDelta();
            Aiming.instance.StopAiming();
            return true;
        }

        return false;
    }

    private bool HandleCancel()
    {
        if (!cancel) return false;
        cancel = Input.GetMouseButton(0);
        return true;
    }

    private Vector2 GetCurrentPosition()
    {
        return Save.Instance.StickToBall() ? (Vector2)Camera.main.WorldToScreenPoint(transform.position) : (Vector2)Input.mousePosition;
    }

    // Returns vector clamped by minimal and maximal speed 
    private Vector2 CalculateVector(Vector2 vector)
    {
        vector /= 20;

        if (vector.magnitude > maxSpeed)
        {
            vector = vector.normalized * maxSpeed;
        }

        const float minDistance = 1f;

        return vector.magnitude > minDistance ? vector : Vector2.zero;
    }

    private void MoveBall(Vector2 direction)
    {
        // Cancel stroke
        if (direction == Vector2.zero)
            return;

        SoundManager.instance.PlayBounce(10);
        rb.AddForce(direction);
        UIManager.Instance.AddStroke();
    }

    void OnCollisionStay2D(Collision2D col)
    {
        // Manual slowdown (surface drag)
        // Circle is assumed to be prefect so it avoids build-in friction
        if (Physics2D.simulationMode == SimulationMode2D.Script)
            return;

        // Could be used for different frictions (ice)
        //    float f = col.collider.sharedMaterial.friction;


        rb.velocity *= .98f;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (Physics2D.simulationMode == SimulationMode2D.Script)
            return;

        SoundManager.instance.PlayBounce(col.relativeVelocity.magnitude);

        ContactPoint2D contact = col.GetContact(0);

        ParticleController.instance.PlayParticles(contact.point, contact.normal, col.relativeVelocity.magnitude);
    }
}
