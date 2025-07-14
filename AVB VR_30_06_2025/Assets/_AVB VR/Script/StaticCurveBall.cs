using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaticCurveBall : MonoBehaviour
{
    public Transform[] waypoints;  // Waypoints for path
    public float speed = 5f;       // Ball movement speed
    public float spinRate = 20f;   // Spin speed (causes curve)
    public LineRenderer lineRenderer;

    private Rigidbody rb;
    private int currentWaypointIndex = 0;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ResetBall();
        DrawTrajectory();
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            MoveAlongPath();
            ApplyMagnusEffect();
        }
    }

    void MoveAlongPath()
    {
        if (currentWaypointIndex >= waypoints.Length)
        {
            isMoving = false;
            return;
        }

        // Move towards the current waypoint
        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        rb.linearVelocity = moveDirection * speed;

        // Check if the ball reached the waypoint
        if (Vector3.Distance(transform.position, targetPosition) < 0.2f)
        {
            currentWaypointIndex++;
        }
    }

    void ApplyMagnusEffect()
    {
        // Magnus Force = Spin x Velocity (Cross Product)
        Vector3 magnusForce = Vector3.Cross(rb.angularVelocity, rb.linearVelocity) * 0.1f;
        rb.AddForce(magnusForce, ForceMode.Force);
    }


    [ContextMenu("StartMoving")]
    public void StartMoving()
    {
        if (isMoving) return;
        isMoving = true;
        rb.isKinematic = false;
        rb.angularVelocity = Vector3.up * spinRate; // Apply spin
    }

    [ContextMenu("ResetBall")]
    public void ResetBall()
    {
        isMoving = false;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Place ball at first waypoint
        transform.position = waypoints[0].position;
        transform.rotation = waypoints[0].rotation;

        currentWaypointIndex = 1; // Start at second waypoint
        DrawTrajectory();
    }

    void DrawTrajectory()
    {
        if (lineRenderer == null) return;

        Vector3[] pathPositions = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            pathPositions[i] = waypoints[i].position;
        }

        lineRenderer.positionCount = waypoints.Length;
        lineRenderer.SetPositions(pathPositions);
    }
}
