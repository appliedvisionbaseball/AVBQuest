using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // Assign a UI Text element in Inspector
    public float countdownTime = 3f; // Countdown start time

    public delegate void CountdownFinished();
    public event CountdownFinished OnCountdownFinished;

    public void StartCountDown()
    {
        AudioHandler.instance.Crowd_AudioPlay();

        countdownText.gameObject.SetActive(true);
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        float timeLeft = countdownTime;

        while (timeLeft > 0)
        {
            countdownText.text = Mathf.Ceil(timeLeft).ToString();
            AudioHandler.instance.CountDownTick_AudioPlay();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        //countdownText.text = "";
        countdownText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);

        OnCountdownFinished?.Invoke(); // Trigger event when countdown ends
    }
}
