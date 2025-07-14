using System.Collections;
using UnityEngine;

public class BezierBallMovement : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform startPoint, controlPoint, endPoint;
    public int curveResolution = 20; // More points = smoother curve
    public float speed = 5f;         // Ball speed along curve

    private Vector3[] curvePoints;
    private int currentPointIndex = 0;
    private bool isMoving = false;

    public GameObject nextPitch;

    void Start()
    {
        //DrawBezierCurve();
        transform.position = startPoint.position; // Place ball at start
    }

    [ContextMenu("Draw Bezier Curve")]
    public void DrawBezierCurve()
    {
        lineRenderer.widthMultiplier = 0.01f;
        curvePoints = new Vector3[curveResolution];
        lineRenderer.positionCount = curveResolution;

        for (int i = 0; i < curveResolution; i++)
        {
            float t = i / (float)(curveResolution - 1);
            curvePoints[i] = CalculateBezierPoint(t, startPoint.position, controlPoint.position, endPoint.position);
            lineRenderer.SetPosition(i, curvePoints[i]);
        }
    }

    [ContextMenu("Start Moving")]
    public void StartMoving()
    {
        if (isMoving) return;
        isMoving = true;
        DrawBezierCurve();
        StartCoroutine(MoveBall());
    }

    private IEnumerator MoveBall()
    {
        while (currentPointIndex < curvePoints.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, curvePoints[currentPointIndex], speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, curvePoints[currentPointIndex]) < 0.1f)
            {
                currentPointIndex++;
            }

            yield return null;
        }

        isMoving = false; // Stop movement when reaching the last point

        yield return new WaitForSeconds(1f);
        lineRenderer.widthMultiplier = 0.1f;

        yield return new WaitForSeconds(5f);
        nextPitch.gameObject.SetActive(true);
    }

    [ContextMenu("Reset Ball")]
    public void ResetBall()
    {
        StopAllCoroutines();
        GetComponent<MeshRenderer>().enabled = false;
        isMoving = false;
        currentPointIndex = 0;
        transform.position = startPoint.position;
        //DrawBezierCurve(); // Redraw trajectory
        ClearLine();
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return (u * u * p0) + (2 * u * t * p1) + (t * t * p2);
    }

    public void ClearLine()
    {
        if (lineRenderer != null)
            lineRenderer.positionCount = 0;
    }

}
