using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameStarter : MonoBehaviour
{
    [Header("Script Reference")]
    public CountdownTimer countdownTimer; // Assign in Inspector
    public BaseballController ballController; // Assign the ball that should move
    public BaseballControllerForPostup ballControllerForPostup;

    [Header("Gameobject")]
    public GameObject budVideoPlayer;
    public LineRenderer trajectory;

    [Header("UI Gameobject")]
    public GameObject nextPitchUI;
    public GameObject backToHomeUI;
    public GameObject speedOMeter;
    public GameObject startRoundPopup;
    public GameObject pitcherDetailPopup;
    public GameObject pitchCounterHeader;
    public GameObject performanceRatingPopup;
    public GameObject clockPopup;
    public GameObject timerPopup;
    public GameObject roundStatPopup;
    public GameObject pitchTypeScorePopup;
    public GameObject pitchInfoPopup;
    public GameObject roundSubPopup;
    public GameObject countDownPopup;

    [Header("Canvas Gameobject")]
    public GameObject UIMainCanvas;
    public GameObject UICanvasInGame;
    public GameObject strikeZone;
    public GameObject measureObject;
    public GameObject nextPitchButton;

    [Header("Pitch Text")]
    public TextMeshProUGUI ballPitchCount;
    public TextMeshProUGUI pitchText;
    public TextMeshProUGUI pitchMeterText;
    public int ballCount = 0;
    public int totalBallCount = 10;

    [Header("Round Text")]
    public TextMeshProUGUI roundCountText;
    public TextMeshProUGUI roundCountText2;
    public int roundCount = 0;

    public int triggerFrame = 100;   // Frame number to trigger the throw
    private bool hasThrown = true;  // Prevent multiple throws

    [Header("Visual Drill")]
    public VisualDrillHandler visualDrillHandler;

    [Header("Postup Drill")]
    public PostupDrillResultHandler postupDrillHandler;

    [Header("PitchType Drill")]
    public PitchTypeDrillResultHandler pitchTypeDrillHandler;

    [Header("SwingTrigger Drill")]
    public SwingTriggerResultHandler swingTriggerHandler;


    void Start()
    {

        pitchText.text = "PITCH : " + ballCount + "/" + totalBallCount;
        pitchMeterText.text = "PITCH : " + ballCount + "/" + totalBallCount;
        ballPitchCount.text = ballCount + "/" + totalBallCount;

        roundCount++;
        roundCountText.text = "Round " + roundCount;
        roundCountText2.text = "Round " + roundCount;

        countdownTimer.OnCountdownFinished += StartGame;
    }

    void StartGame()
    {
        Debug.Log("Countdown finished! Starting the game...");

        // Example: Enable ball movement
        if (ballController != null)
        {
            budVideoPlayer.GetComponent<VideoPlayer>().targetTexture.Release();
            budVideoPlayer.SetActive(true);
            hasThrown = false;
            //StartCoroutine(WaitForBallThrow());
        }
        else if (ballControllerForPostup != null)
        {
            budVideoPlayer.GetComponent<VideoPlayer>().targetTexture.Release();
            budVideoPlayer.SetActive(true);
            hasThrown = false;
        }
    }

    void OnDestroy()
    {
        countdownTimer.OnCountdownFinished -= StartGame; // Unsubscribe to avoid memory leaks
    }

    //IEnumerator WaitForBallThrow()
    //{
    //    yield return new WaitForSeconds(3.5f);

    //    ballController.baseballTransform.GetComponent<MeshRenderer>().enabled = true;
    //    ballController.BaseballValueSetup_BallThrow();

    //    AudioHandler.instance.BallWoosh_AudioPlay();

    //    ballCount++;

    //    pitchText.text = "PITCH : " + ballCount + "/" + totalBallCount;
    //    ballPitchCount.text = ballCount + "/" + totalBallCount;
    //    pitchMeterText.text = "PITCH : " + ballCount + "/" + totalBallCount;

    //    roundCountText.text = "Round " + roundCount;
    //    roundCountText2.text = "Round " + roundCount;

    //    StartCoroutine(ShowNextPitchUI());
    //}

    void Update()
    {
        if (budVideoPlayer.GetComponent<VideoPlayer>().isPlaying && !hasThrown)
        {
            // Check if the current video frame matches the trigger frame
            if ((long)budVideoPlayer.GetComponent<VideoPlayer>().frame >= triggerFrame)
            {
                if (ballController != null)
                {
                    ballController.baseballTransform.GetComponent<MeshRenderer>().enabled = true;
                    ballController.BaseballValueSetup_BallThrow();
                }
                else if (ballControllerForPostup != null)
                {
                    ballControllerForPostup.baseballTransform.GetComponent<MeshRenderer>().enabled = true;
                    ballControllerForPostup.BaseballValueSetup_BallThrow();
                }

                AudioHandler.instance.BallWoosh_AudioPlay();

                ballCount++;

                pitchText.text = "PITCH : " + ballCount + "/" + totalBallCount;
                ballPitchCount.text = ballCount + "/" + totalBallCount;
                pitchMeterText.text = "PITCH : " + ballCount + "/" + totalBallCount;

                roundCountText.text = "Round " + roundCount;
                roundCountText2.text = "Round " + roundCount;

                if (visualDrillHandler != null)
                {
                    visualDrillHandler.AssignCShape_ToBall();
                    StartCoroutine(ShowNextPitchUI());
                }
                else
                {
                    StartCoroutine(ShowNextPitchUI());
                }
                hasThrown = true; // Prevent multiple throws
            }
        }
    }

    IEnumerator ShowNextPitchUI()
    {
        yield return new WaitForSeconds(5f);
        budVideoPlayer.SetActive(false);

        if (ballCount != totalBallCount)
        {
            ActiveUI();
        }
        else
        {
            roundCount++;

            ActiveUI();
            nextPitchButton.SetActive(false);

            yield return new WaitForSeconds(5f);
            AudioHandler.instance.Crowd_AudioStop();

            if (visualDrillHandler != null)
            {
                yield return null;
            }
            else if (postupDrillHandler != null)
            {
                postupDrillHandler.CorrectAnsText.gameObject.SetActive(false);

                speedOMeter.SetActive(false);
                trajectory.gameObject.SetActive(false);

                postupDrillHandler.DisplayResults();

                backToHomeUI.SetActive(true);
                roundSubPopup.SetActive(true);
                measureObject.SetActive(false);

                if (ballController != null)
                {
                    ballController.RingIndicatorOnTrigger.SetActive(false);
                    ballController.RingIndicatorOnStart.SetActive(false);
                    ballController.baseballTransform.GetComponent<MeshRenderer>().enabled = false;
                }
                else if (ballControllerForPostup != null)
                {
                    ballControllerForPostup.RingIndicatorOnTrigger.SetActive(false);
                    ballControllerForPostup.RingIndicatorOnStart.SetActive(false);
                    ballControllerForPostup.baseballTransform.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            else if (pitchTypeDrillHandler != null)
            {
                yield return null;
            }
            else if (swingTriggerHandler != null)
            {
                speedOMeter.SetActive(false);
                trajectory.gameObject.SetActive(false);

                swingTriggerHandler.DisplayResults();

                backToHomeUI.SetActive(true);
                roundSubPopup.SetActive(true);
                measureObject.SetActive(false);

                if (ballController != null)
                {
                    ballController.RingIndicatorOnTrigger.SetActive(false);
                    ballController.RingIndicatorOnStart.SetActive(false);
                    ballController.baseballTransform.GetComponent<MeshRenderer>().enabled = false;
                }
                else if (ballControllerForPostup != null)
                {
                    ballControllerForPostup.RingIndicatorOnTrigger.SetActive(false);
                    ballControllerForPostup.RingIndicatorOnStart.SetActive(false);
                    ballControllerForPostup.baseballTransform.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
    }

    public void ShowResultPanelAfterButtonClick()
    {
        strikeZone.SetActive(true);

        if (ballCount >= totalBallCount)
            StartCoroutine(ShowResultUIAfterClick());
    }

    IEnumerator ShowResultUIAfterClick()
    {
        yield return new WaitForSeconds(5f);

        backToHomeUI.SetActive(true);
        roundSubPopup.SetActive(true);

        speedOMeter.SetActive(false);
        trajectory.gameObject.SetActive(false);

        if (visualDrillHandler != null)
        {
            visualDrillHandler.CorrectAns.gameObject.SetActive(false);
            visualDrillHandler.ResultPanel();
            measureObject.SetActive(false);

            if (ballController != null)
            {
                ballController.RingIndicatorOnTrigger.SetActive(false);
                ballController.RingIndicatorOnStart.SetActive(false);
                ballController.baseballTransform.GetComponent<MeshRenderer>().enabled = false;
            }
            else if (ballControllerForPostup != null)
            {
                ballControllerForPostup.RingIndicatorOnTrigger.SetActive(false);
                ballControllerForPostup.RingIndicatorOnStart.SetActive(false);
                ballControllerForPostup.baseballTransform.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else if (postupDrillHandler != null)
        {
            postupDrillHandler.DisplayResults();
        }
        else if (pitchTypeDrillHandler != null)
        {
            pitchTypeDrillHandler.CorrectAnsText.gameObject.SetActive(false);
            pitchTypeDrillHandler.DisplayResults();
            measureObject.SetActive(false);

            pitchInfoPopup.SetActive(true);

            if (ballController != null)
            {
                ballController.RingIndicatorOnTrigger.SetActive(false);
                ballController.RingIndicatorOnStart.SetActive(false);
                ballController.baseballTransform.GetComponent<MeshRenderer>().enabled = false;
            }
            else if (ballControllerForPostup != null)
            {
                ballControllerForPostup.RingIndicatorOnTrigger.SetActive(false);
                ballControllerForPostup.RingIndicatorOnStart.SetActive(false);
                ballControllerForPostup.baseballTransform.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else if (swingTriggerHandler != null)
        {
            swingTriggerHandler.DisplayResults();
        }
    }

    IEnumerator ShowVirtualDrilButtonUI()
    {
        yield return new WaitForSeconds(5f);

        if (ballCount != totalBallCount)
        {
            ActiveUI();
        }
        else
        {
            AudioHandler.instance.Crowd_AudioStop();
            roundCount++;

            ActiveUI();
            nextPitchButton.SetActive(false);

            yield return new WaitForSeconds(5f);

            backToHomeUI.SetActive(true);
            roundSubPopup.SetActive(true);

            if (visualDrillHandler != null)
            {
                visualDrillHandler.ResultPanel();
            }
        }
    }

    public void ResetBallCount()
    {
        ballCount = 0;
        pitchText.text = "PITCH : " + ballCount + "/" + totalBallCount;
        ballPitchCount.text = ballCount + "/" + totalBallCount;
        pitchMeterText.text = "PITCH : " + ballCount + "/" + totalBallCount;

        roundCountText.text = "Round " + roundCount;
        roundCountText2.text = "Round " + roundCount;

        if (ballController != null)
        {
            ballController.ResetValue();
        }
        else if (ballControllerForPostup != null)
        {
            ballControllerForPostup.ResetValue();
        }
    }

    public void ResetPitch()
    {
        StopAllCoroutines();
        countdownTimer.StopAllCoroutines();
        
        budVideoPlayer.SetActive(false);

        HideUI();

        trajectory.positionCount = 0;
        speedOMeter.SetActive(false);
       
        if (ballController != null)
        {
            ballController.baseballTransform.GetComponent<MeshRenderer>().enabled = false;
            ballController.StopAllCoroutines();
            ballController.RingIndicatorOnStart.SetActive(false);
            ballController.RingIndicatorOnTrigger.SetActive(false);
        }
        else if (ballControllerForPostup != null)
        {
            ballControllerForPostup.baseballTransform.GetComponent<MeshRenderer>().enabled = false;
            ballControllerForPostup.StopAllCoroutines();
            ballControllerForPostup.RingIndicatorOnStart.SetActive(false);
            ballControllerForPostup.RingIndicatorOnTrigger.SetActive(false);
        }
    }

    public void ActiveUI()
    {
        if (visualDrillHandler != null)
        {
            visualDrillHandler.cSelectionPanel.SetActive(true);
            visualDrillHandler.BallButtonInteractable();

            strikeZone.SetActive(true);
        }

        else if (pitchTypeDrillHandler != null)
        {
            pitchTypeDrillHandler.ButtonInteractable();
            nextPitchUI.SetActive(true);
        }
        else
        {
            nextPitchUI.SetActive(true);
            measureObject.SetActive(true);

            strikeZone.SetActive(true);
        }


        if (pitchTypeDrillHandler != null)
        {
            pitchCounterHeader.SetActive(true);
            performanceRatingPopup.SetActive(true);
            clockPopup.SetActive(true);
            timerPopup.SetActive(true);
            roundStatPopup.SetActive(true);
            pitchTypeScorePopup.SetActive(true);
        }
        else
        {
            //startRoundPopup.SetActive(true);
            pitchCounterHeader.SetActive(true);
            performanceRatingPopup.SetActive(true);
            clockPopup.SetActive(true);
            timerPopup.SetActive(true);
            roundStatPopup.SetActive(true);
            pitchTypeScorePopup.SetActive(true);
            pitchInfoPopup.SetActive(true);
        }

        if (postupDrillHandler != null)
        {
            postupDrillHandler.CheckAnswer();
            speedOMeter.SetActive(true);
        }
    }

    public void HideUI()
    {
        roundSubPopup.SetActive(false);
        strikeZone.SetActive(false);


        if (visualDrillHandler != null)
        {
            visualDrillHandler.cSelectionPanel.SetActive(false);
            visualDrillHandler.CorrectAns.gameObject.SetActive(false);
        }
        else if (pitchTypeDrillHandler != null)
        {
            pitchTypeDrillHandler.CorrectAnsText.gameObject.SetActive(false);
            nextPitchUI.SetActive(false);
        }
        else if (postupDrillHandler != null)
        {
            postupDrillHandler.CorrectAnsText.gameObject.SetActive(false);
            nextPitchUI.SetActive(false);
            measureObject.SetActive(false);
        }
        else
        {
            nextPitchUI.SetActive(false);
            measureObject.SetActive(false);
        }

        startRoundPopup.SetActive(false);
        pitcherDetailPopup.SetActive(false);
        //pitchCounterHeader.SetActive(false);
        performanceRatingPopup.SetActive(false);
        clockPopup.SetActive(false);
        timerPopup.SetActive(false);
        roundStatPopup.SetActive(false);
        pitchTypeScorePopup.SetActive(false);
        pitchInfoPopup.SetActive(false);
        countDownPopup.SetActive(false);
    }

    public void HomeButtonClick()
    {
        PauseGame();
        HideUI();
        ResetPitch();

        backToHomeUI.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // Pause all game physics, animations, and movement
        //BaseballController.isTweening = false; // Stop ball movement
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Resume game
        //BaseballController.isTweening = true;  // Allow ball movement again
        Debug.Log("Game Resumed");
    }

    public void PlayAgain_ButtonClick()
    {
        AudioHandler.instance.Crowd_AudioStop();

        nextPitchButton.SetActive(true);

        ResetBallCount();
        ResetPitch();
        HideUI();
        pitchCounterHeader.SetActive(false);
        backToHomeUI.gameObject.SetActive(false);

        startRoundPopup.SetActive(true);
        pitcherDetailPopup.SetActive(true);
        ResumeGame();

        if (visualDrillHandler != null)
        {
            visualDrillHandler.ResetResult();
        }
        else if (postupDrillHandler != null)
        {
            postupDrillHandler.ResetResult();
        }
        else if (pitchTypeDrillHandler != null)
        {
            pitchTypeDrillHandler.ResetResult();
        }
        else if (swingTriggerHandler != null)
        {
            swingTriggerHandler.ResetResult();
        }

    }

    public void Home_ButtonClick()
    {
        //AudioHandler.instance.Crowd_AudioStop();

        //nextPitchButton.SetActive(true);

        //ResetBallCount();
        //ResetPitch();
        //HideUI();
        //pitchCounterHeader.SetActive(false);
        //backToHomeUI.gameObject.SetActive(false);

        //UIMainCanvas.SetActive(true);

        //startRoundPopup.SetActive(true);
        //pitcherDetailPopup.SetActive(true);
        //UICanvasInGame.SetActive(false);

        //ResumeGame();

        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
