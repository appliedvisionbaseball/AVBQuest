using UnityEngine;
using TMPro;

public class StopwatchTimer : MonoBehaviour
{
    [Header("Timer")]
    public float elapsedTime = 0f;
    public bool isTimerRunning = false;
    public TextMeshProUGUI timerText;

    [Header("Clock")]
    public TextMeshProUGUI clockText; // Assign in Unity Inspector

    void Start()
    {
        isTimerRunning = true;
        InvokeRepeating("UpdateClock", 0f, 1f); // Update time every second
    }

    void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}"; // Format as MM:SS
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    void UpdateClock()
    {
        clockText.text = System.DateTime.Now.ToString("hh:mm:ss tt"); // Format: 12-hour (AM/PM)
    }
}
