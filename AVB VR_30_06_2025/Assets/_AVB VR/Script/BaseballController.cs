using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using BNG;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class BaseballController : MonoBehaviour
{
    [Header("Main Component")]
    public VideoPlayer videoPlayer;
    public Transform baseballTransform;
    public LineRenderer lineRenderer;
    public Material lineRendereMat;

    [Header("UI Component")]
    public GameObject speedOMeterObject;
    public TextMeshProUGUI speedOMeterText;
    public TextMeshProUGUI recognitionWindowText;
    public TextMeshProUGUI pitchCalledText;
    public TextMeshProUGUI hitterActionText;
    public TextMeshProUGUI decisionText;
    public TextMeshProUGUI decisionAccuracyText;
    public TextMeshProUGUI ballDistanceMeterText;
    public TextMeshProUGUI ballDistnaceTimeText;
    public TextMeshProUGUI pitchTypeText;
    public TextMeshProUGUI ballSpeedText;
    public TextMeshProUGUI windowRecogText;
    public TextMeshProUGUI pitchTypeAvgText;
    public TextMeshProUGUI pitchTypeAvgFontText;
    public TextMeshProUGUI accuracyAvgText;
    public TextMeshPro optimalText;
    public TextMeshPro yourDecisionText;
    public GameObject yourDecisionObject;
    public GameObject lineMeasureObject;
    public GameObject yourDecisionDotObject;

    [Header("Max Value UI")]
    public TextMeshProUGUI maxBallDistanceText;
    public TextMeshProUGUI maxTravelTimeText;
    public TextMeshProUGUI maxAccuracyAvgText;
    public TextMeshProUGUI maxDecisionAccText;
    private float maxDecisionAccuracy = 0f;
    private float currentDecisionAccuracy = 0f;

    [Header("Start Position of ball based on Pitchers")]
    public Transform Steve;
    public Transform budSmith;
    public Transform Dante;
    public Transform Chris;
    public Transform Brian;
    public Transform Hiro;
    public Transform Steve2;

    [Header("Ball points")]
    public Transform handleDefault;
    public List<Transform> handleDefaultSlider;
    public List<Transform> handleDefaultCurve;
    public List<Transform> handleDefaultChangeUp;
    public List<Transform> handleEndDefaultChangeUp;
    public List<Transform> handleEndDefaultCurve;

    [Header("Twining")]
    public float percent = 0;
    public int curveResolution = 10;
    [Range(.1f, 10)] public float tweenLength = 3;
    public AnimationCurve tweenSpeed;
    public float tweenTimer = 0;
    public static bool isTweening = false;

    [Header("Assign Defaul")]
    public Transform startTransform;
    public Transform endTransform;
    public Transform handleTransform;
    public Transform handleEndTransform;
    public GameObject movingObject;

    [Header("Update on runtime")]
    public string pitchType;
    public string BallType;
    public int pitchTypes;
    public float baseballSpeedMPH;
    public float playerDecisionForDistance;
    public Vector3 endPoses;
    float[] values = { 1.8f, 2.5f, 3f, 3.8f, 4.5f, 3.5f, 2f, 5f, 1.5f };
    public float endPoseForline;
    public float speedMultiplier = 5;
    public List<GameObject> Balls = new List<GameObject>(); //Balls: A pitch that does not pass through this defined strike zone is called a ball.
    public List<GameObject> Strikes = new List<GameObject>(); //Strikes: A pitch that passes through any part of this zone
    private List<string> lastTwoPitches = new List<string>(); // Track last two pitch types

    [Header("Update based on UI seletion")]
    public int pitchersSetting;  // Steve, Bud, Dante, Chris, Brian, Hiro, Steve2
    public int velocitySetting; // 50-55, 55-60, 60-65, 65-70, 75-80, 80-85, 85-90, 90-95, 95-100, 98-103
    public int accuracySetting; // PinPoint = 95%, Accurate = 90%, Finding the zone = 80%, Wild = 50%
    public int drillsetting;    // Visual Acuity, Pitch Type, Swing Trigger, Post Up, Free Round

    [Header("Swing")]
    public bool isSwing = false;
    float idealSwingTime = 2f;   // Expected time to hit
    float swingStartTime;         // Time when player swings
    float timingAccuracy;

    [Header("Pitch Rating")]
    public float velocityScore;
    public float accuracyScore;
    public float effectivenessScore;
    public float finalRating;

    bool isEndLoop = false;

    [Header("Strike Tracking")]
    public int totalPitches = 0;
    public int totalStrikes = 0;
    public List<bool> last10Throws = new List<bool>(); // Track last 10 pitch results
    public float strikeAverage = 0;

    [Header("Accuracy Tracking")]
    public List<float> last10Accuracies = new List<float>(); // Store last 10 accuracy scores
    public float averageAccuracy = 0;

    [Header("Meter Arrow")]
    public GameObject roundRatingArrow;
    public GameObject pitchRatingArrow;
    private float roundRatingArrowXValue;

    [Header("Grid")]
    public int gridCounter;
    public GameObject[] gridHighlight;

    [Header("Pitcher Detail")]
    public TextMeshProUGUI pitcherName;
    public TextMeshProUGUI pitcherName2;
    public TextMeshProUGUI pitcherHeight;
    public TextMeshProUGUI pitcherArm;
    public TextMeshProUGUI pitcherAngle;
    public TextMeshProUGUI pitcherStyle;
    public Image pitcherImage;

    [Header("Trigger ball")]
    public GameObject RingIndicatorOnStart;
    public GameObject RingIndicatorOnTrigger;

    [Header("Decision Time")]
    bool isStartTime = false;
    DateTime startTime;
    public float optimalTime, yourTime = 0f;
    public float decisionMinValue, decisionMaxValue;
    public float minTimeToPlate, maxTimeToPlate;
    public float currentTimeToPlate;
    public float minFastSpeed, maxFastSpeed, currentFastSpeed;
    public float minSliderSpeed, maxSliderSpeed, currentSliderSpeed;
    public float minChangeupSpeed, maxChangeupSpeed, currentChangeupSpeed;
    public float minCurveBallSpeed, maxCurveBallSpeed, currentCurveBallSpeed;
    public TextMeshProUGUI decisionTypeText;

    [Header("New Line")]
    public GameObject dotObject1;
    public GameObject dotObject2;
    public LineRenderer lineMeasure;

    [Header("Visual Drill")]
    public VisualDrillHandler visualDrillHandler;
    public bool isVirtualDrill = false;

    [Header("Swing Trigger Drill")]
    public SwingTriggerResultHandler swingTriggerResultHandler;

    private List<Vector3> endPosesHistory = new List<Vector3>();
    private int MAX_CONSECUTIVE = 2;


    void Start()
    {
        AssignPitcher();
        AssignAccuracy();
        AssignVelocity();
        roundRatingArrowXValue = roundRatingArrow.transform.localPosition.x;
    }

    float baseSpeedMultiplier;
    void Update()
    {
        if (isTweening)
        {
            if (!isStartTime)
            {
                isStartTime = true;
                startTime = System.DateTime.Now;
                Debug.Log("---> startTime time : " + startTime.ToString("mm:ss.fff"));
            }

            // Check if right trigger is pressed
            if ((InputBridge.Instance.RightTriggerDown || Input.GetKeyDown(KeyCode.R) || (InputBridge.Instance.LeftTriggerDown) || Input.GetKeyDown(KeyCode.L)) && !isVirtualDrill)
            {
                Debug.Log("Right Controller Trigger Pressed");

                int ToHr = DateTime.Now.Hour - startTime.Hour;
                int ToMin = DateTime.Now.Minute - startTime.Minute;
                int ToSec = DateTime.Now.Second - startTime.Second;
                int ToMiSec = DateTime.Now.Millisecond - startTime.Millisecond;
                int ToMilisec = ((((ToHr * 60) + (ToMin)) * 60) + ToSec) * 1000;

                yourTime = (ToMilisec + ToMiSec) / 1000f;

                if (yourTime < maxTimeToPlate - 0.01f)
                {
                    Debug.Log("yourTime is within the valid range");

                    Debug.Log("--> Swing time : " + (yourTime) + "--" + DateTime.Now.ToString("mm:ss.fff") + "--" + startTime.ToString("mm:ss.fff"));

                    swingStartTime = Time.time;

                    playerDecisionForDistance = Vector3.Distance(startTransform.position, baseballTransform.position);

                    RingIndicatorOnTrigger.transform.localPosition = new Vector3(baseballTransform.position.x, baseballTransform.position.y, baseballTransform.position.z);
                    //RingIndicatorOnTrigger.SetActive(true);

                    CalculateTimingAccuracy();
                    isSwing = true;

                    Debug.Log("plyerswing time : " + swingStartTime + "--" + baseballTransform.position + "--" + playerDecisionForDistance);
                }
                else
                {
                    Debug.Log("yourTime is out of range " + yourTime + "--" + decisionMaxValue);
                }
            }

            switch (velocitySetting)
            {
                case 0: //50-55
                    speedMultiplier = 4f;
                    decisionMinValue = 0.225f;
                    decisionMaxValue = 0.247f;
                    minTimeToPlate = 0.750f; maxTimeToPlate = 0.825f;

                    minFastSpeed = 50f; maxFastSpeed = 55f;
                    minSliderSpeed = 46f; maxSliderSpeed = 51f;
                    minChangeupSpeed = 45f; maxChangeupSpeed = 50f;
                    minCurveBallSpeed = 42; maxCurveBallSpeed = 47f;

                    baseSpeedMultiplier = speedMultiplier;
                    // Apply 10% reduction to speedMultiplier for Changeup pitches
                    speedMultiplier = pitchType == "Changeup" ? baseSpeedMultiplier * 0.9f : baseSpeedMultiplier;

                    break;

                case 1: //55-60
                    speedMultiplier = 4.4f;
                    decisionMinValue = 0.206f;
                    decisionMaxValue = 0.225f;
                    minTimeToPlate = 0.687f; maxTimeToPlate = 0.750f;

                    minFastSpeed = 55f; maxFastSpeed = 60f;
                    minSliderSpeed = 51f; maxSliderSpeed = 55f;
                    minChangeupSpeed = 50f; maxChangeupSpeed = 54f;
                    minCurveBallSpeed = 47; maxCurveBallSpeed = 51f;

                    baseSpeedMultiplier = speedMultiplier;
                    // Apply 10% reduction to speedMultiplier for Changeup pitches
                    speedMultiplier = pitchType == "Changeup" ? baseSpeedMultiplier * 0.9f : baseSpeedMultiplier;
                    break;

                case 2: //60-65
                    speedMultiplier = 4.8f;
                    decisionMinValue = 0.190f;
                    decisionMaxValue = 0.206f;
                    minTimeToPlate = 0.634f; maxTimeToPlate = 0.687f;

                    minFastSpeed = 60f; maxFastSpeed = 65f;
                    minSliderSpeed = 55f; maxSliderSpeed = 60f;
                    minChangeupSpeed = 54f; maxChangeupSpeed = 58f;
                    minCurveBallSpeed = 51; maxCurveBallSpeed = 55f;

                    baseSpeedMultiplier = speedMultiplier;
                    // Apply 10% reduction to speedMultiplier for Changeup pitches
                    speedMultiplier = pitchType == "Changeup" ? baseSpeedMultiplier * 0.9f : baseSpeedMultiplier;
                    break;

                case 3: //65-70
                    speedMultiplier = 5.6f;
                    decisionMinValue = 0.165f;
                    decisionMaxValue = 0.190f;
                    minTimeToPlate = 0.687f; maxTimeToPlate = 0.550f;

                    minFastSpeed = 65f; maxFastSpeed = 70f;
                    minSliderSpeed = 60f; maxSliderSpeed = 64f;
                    minChangeupSpeed = 58f; maxChangeupSpeed = 63f;
                    minCurveBallSpeed = 55; maxCurveBallSpeed = 60f;

                    baseSpeedMultiplier = speedMultiplier;
                    // Apply 10% reduction to speedMultiplier for Changeup pitches
                    speedMultiplier = pitchType == "Changeup" ? baseSpeedMultiplier * 0.9f : baseSpeedMultiplier;
                    break;

                case 4: //75-80
                    speedMultiplier = 5.9f;
                    decisionMinValue = 0.155f;
                    decisionMaxValue = 0.163f;
                    minTimeToPlate = 0.516f; maxTimeToPlate = 0.543f;

                    minFastSpeed = 75f; maxFastSpeed = 80f;
                    minSliderSpeed = 69f; maxSliderSpeed = 74f;
                    minChangeupSpeed = 68f; maxChangeupSpeed = 72f;
                    minCurveBallSpeed = 64; maxCurveBallSpeed = 68f;

                    baseSpeedMultiplier = speedMultiplier;
                    // Apply 10% reduction to speedMultiplier for Changeup pitches
                    speedMultiplier = pitchType == "Changeup" ? baseSpeedMultiplier * 0.9f : baseSpeedMultiplier;

                    break;

                case 5: //80-85
                    speedMultiplier = 6.3f;
                    decisionMinValue = 0.146f;
                    decisionMaxValue = 0.155f;
                    minTimeToPlate = 0.485f; maxTimeToPlate = 0.516f;

                    minFastSpeed = 80f; maxFastSpeed = 85f;
                    minSliderSpeed = 74f; maxSliderSpeed = 78f;
                    minChangeupSpeed = 72f; maxChangeupSpeed = 76f;
                    minCurveBallSpeed = 68; maxCurveBallSpeed = 72f;

                    baseSpeedMultiplier = speedMultiplier;
                    // Apply 10% reduction to speedMultiplier for Changeup pitches
                    speedMultiplier = pitchType == "Changeup" ? baseSpeedMultiplier * 0.9f : baseSpeedMultiplier;

                    break;

                case 6: //85-90
                    speedMultiplier = 6.6f;
                    decisionMinValue = 0.137f;
                    decisionMaxValue = 0.146f;
                    minTimeToPlate = 0.458f; maxTimeToPlate = 0.485f;

                    minFastSpeed = 85f; maxFastSpeed = 90f;
                    minSliderSpeed = 78f; maxSliderSpeed = 83f;
                    minChangeupSpeed = 76f; maxChangeupSpeed = 81f;
                    minCurveBallSpeed = 72; maxCurveBallSpeed = 76f;

                    baseSpeedMultiplier = speedMultiplier;
                    // Apply 10% reduction to speedMultiplier for Changeup pitches
                    speedMultiplier = pitchType == "Changeup" ? baseSpeedMultiplier * 0.9f : baseSpeedMultiplier;

                    break;

                case 7: //90-95
                    speedMultiplier = 7f;
                    decisionMinValue = 0.130f;
                    decisionMaxValue = 0.137f;
                    minTimeToPlate = 0.434f; maxTimeToPlate = 0.458f;

                    minFastSpeed = 90f; maxFastSpeed = 95f;
                    minSliderSpeed = 83f; maxSliderSpeed = 87f;
                    minChangeupSpeed = 81f; maxChangeupSpeed = 86f;
                    minCurveBallSpeed = 76; maxCurveBallSpeed = 81f;

                    baseSpeedMultiplier = speedMultiplier;
                    // Apply 10% reduction to speedMultiplier for Changeup pitches
                    speedMultiplier = pitchType == "Changeup" ? baseSpeedMultiplier * 0.9f : baseSpeedMultiplier;

                    break;

                case 8: //95-100
                    speedMultiplier = 7.4f;
                    decisionMinValue = 0.124f;
                    decisionMaxValue = 0.130f;
                    minTimeToPlate = 0.412f; maxTimeToPlate = 0.434f;

                    minFastSpeed = 95f; maxFastSpeed = 100f;
                    minSliderSpeed = 87f; maxSliderSpeed = 92f;
                    minChangeupSpeed = 86f; maxChangeupSpeed = 90f;
                    minCurveBallSpeed = 81; maxCurveBallSpeed = 85f;

                    baseSpeedMultiplier = speedMultiplier;
                    // Apply 10% reduction to speedMultiplier for Changeup pitches
                    speedMultiplier = pitchType == "Changeup" ? baseSpeedMultiplier * 0.9f : baseSpeedMultiplier;

                    break;

                case 9: //98-103
                    speedMultiplier = 7.5f;
                    decisionMinValue = 0.120f;
                    decisionMaxValue = 0.124f;
                    minTimeToPlate = 0.400f; maxTimeToPlate = 0.412f;

                    minFastSpeed = 98f; maxFastSpeed = 103f;
                    minSliderSpeed = 90f; maxSliderSpeed = 95f;
                    minChangeupSpeed = 89f; maxChangeupSpeed = 93f;
                    minCurveBallSpeed = 84; maxCurveBallSpeed = 88f;

                    baseSpeedMultiplier = speedMultiplier;
                    // Apply 10% reduction to speedMultiplier for Changeup pitches
                    speedMultiplier = pitchType == "Changeup" ? baseSpeedMultiplier * 0.9f : baseSpeedMultiplier;

                    break;

            }

            // Debug.Log($"this is speedmultiplier {speedMultiplier} and velocitySetting {velocitySetting}");

            // Debug.Log($"Velocity Setting: {velocitySetting}, Speed Multiplier: {speedMultiplier}");

            tweenTimer += Time.deltaTime * speedMultiplier;
            float p = tweenTimer / tweenLength;
            percent = tweenSpeed.Evaluate(p);

            if (tweenTimer > (tweenLength + 0.2f))
            {
                tweenTimer = 0;
                percent = 0;
                isTweening = false;
                isEndLoop = false;
            }
            Vector3 tempBallPos;
            if (tweenTimer == 0 && !isEndLoop)
            {
                Debug.Log("End Loop");

                currentTimeToPlate = Random.Range(minTimeToPlate, maxTimeToPlate);
                currentFastSpeed = Random.Range(minFastSpeed, maxFastSpeed);
                currentSliderSpeed = Random.Range(minSliderSpeed, maxSliderSpeed);
                currentChangeupSpeed = Random.Range(minChangeupSpeed, maxChangeupSpeed);
                currentCurveBallSpeed = Random.Range(minCurveBallSpeed, maxCurveBallSpeed);


                Debug.Log("--> decision value : " + currentTimeToPlate + "--" + currentFastSpeed + "--" + currentSliderSpeed + "--" + currentChangeupSpeed + "--" + currentCurveBallSpeed);

                //if (pitchType == "Changeup")
                //{
                //    tempBallPos = new Vector3(baseballTransform.position.x - 2f, baseballTransform.position.y + 1.04f, baseballTransform.position.z + 2f);
                //}
                //if (pitchType == "Slider")
                //{
                //    tempBallPos = new Vector3(baseballTransform.position.x + 2f, baseballTransform.position.y + 1f, baseballTransform.position.z + 4f);
                //}
                //else
                {
                    tempBallPos = new Vector3(baseballTransform.position.x, baseballTransform.position.y - 0.04f, baseballTransform.position.z);
                }


                baseballTransform.position = tempBallPos;

                float ToHr = DateTime.Now.Hour - startTime.Hour;
                float ToMin = DateTime.Now.Minute - startTime.Minute;
                float ToSec = DateTime.Now.Second - startTime.Second;
                float ToMiSec = DateTime.Now.Millisecond - startTime.Millisecond;

                // Adjust for potential negative values or rollovers
                if (ToMiSec < 0) { ToSec--; ToMiSec += 1000; }
                if (ToSec < 0) { ToMin--; ToSec += 60; }
                if (ToMin < 0) { ToHr--; ToMin += 60; }

                // Convert to milliseconds
                float ToMilisec = ((((ToHr * 60f) + ToMin) * 60f) + ToSec) * 1000f + ToMiSec;

                optimalTime = 0.200f;
                Debug.Log("---> End time : " + optimalTime + "--" + startTime + "--" + DateTime.Now.ToString("mm:ss.fff") + "  " + ToMilisec);

                UpdateTrajectoryPath();
                isEndLoop = true;

                ValueSetupOnUI();
            }
        }

        if (percent > 0)
        {
            //if (pitchTypes == 3 || pitchTypes == 2)
            //{
            //    baseballTransform.position = CalcPositionOnCurveChangeUP(percent);
            //}
            //else
            {
                baseballTransform.position = CalcPositionOnCurve(percent);
            }

            // Debug.Log($"this is tweenSpeed {percent} and tweenTimer {tweenTimer} and tween Lenght is {tweenLength} and tranform.position is {baseballTransform.position}");
        }
    }

    public void ValueSetupOnUI()
    {
        if (visualDrillHandler != null)
            visualDrillHandler.DisableCShape_Ball();

        lineMeasure.positionCount = 0;

        BallDistanceAndTimeCalcultion();
        UpdateStrikeTracking();
        recognitionWindowText.text = "Recognition Window - " + BallType;
        windowRecogText.text = "Recognition Window\n" + BallType;
        pitchCalledText.text = pitchType + " " + BallType + " " + baseballSpeedMPH.ToString("F2") + " MPH";
        pitchTypeText.text = pitchType;
        ballSpeedText.text = baseballSpeedMPH.ToString("F2");

        speedOMeterText.text = pitchType + " " + BallType + "\n" + baseballSpeedMPH.ToString("F2") + " MPH";
        speedOMeterObject.SetActive(true);

        Vector3 newEndPos = new Vector3(endTransform.position.x, endTransform.position.y, endTransform.position.z + endPoseForline);
        float distance = CalculateBallDistance(startTransform.position, newEndPos);

        //optimalText.text = "OPTIMAL DECISION\n" + ConvertFeetToFeetInches(distance) + "\n"+ optimalTime.ToString("F3") + " ms";
        optimalText.text = "OPTIMAL DECISION\n" + optimalTime.ToString("F3") + " ms";

        if (swingTriggerResultHandler != null)
        {
            swingTriggerResultHandler.BallTypeString.Add(pitchType + " " + BallType);
            if (yourTime != 0)
                swingTriggerResultHandler.decisionTime.Add(yourTime.ToString("F3") + " ms");
            else
                swingTriggerResultHandler.decisionTime.Add("--");
        }

        if (!isVirtualDrill)
        {
            if (isSwing == true)
            {
                if (BallType == "Strike")
                {
                    decisionText.text = "Correct";
                    decisionTypeText.color = Color.green;
                }
                else
                {
                    decisionText.text = "Incorrect";
                    decisionTypeText.color = Color.red;
                }

                hitterActionText.text = "Swing";

                currentDecisionAccuracy = distance - playerDecisionForDistance;
                currentDecisionAccuracy = Mathf.Abs(currentDecisionAccuracy);
                decisionAccuracyText.text = ConvertFeetToFeetInches(Mathf.Abs(currentDecisionAccuracy));
                //yourDecisionText.text = "YOUR DECISION\n" + ConvertFeetToFeetInches(playerDecisionForDistance) + "\n" + yourTime.ToString("F3") + " ms";
                yourDecisionText.text = "YOUR DECISION\n" + yourTime.ToString("F3") + " ms";
                float temp = yourTime * 10f;
                temp = -temp  * 3f;

                yourDecisionObject.transform.localPosition = new Vector3(yourDecisionObject.transform.localPosition.x,
                    yourDecisionObject.transform.localPosition.y, (temp ));
                yourDecisionDotObject.transform.localPosition = new Vector3(yourDecisionDotObject.transform.localPosition.x, yourDecisionDotObject.transform.localPosition.y, (temp ));

                lineMeasureObject.transform.localPosition = new Vector3(lineMeasureObject.transform.localPosition.x, ((temp )  / 2), lineMeasureObject.transform.localPosition.z);
                lineMeasureObject.transform.localScale = new Vector3(lineMeasureObject.transform.localScale.x, ( yourDecisionObject.transform.localPosition.y), lineMeasureObject.transform.localScale.z);

                lineMeasure.transform.position = new Vector3(dotObject1.transform.position.x + dotObject2.transform.position.x, 0, dotObject1.transform.position.z + dotObject2.transform.position.z);

                // Set positions
                lineMeasure.positionCount = 2;
                lineMeasure.SetPosition(0,new Vector3( dotObject1.transform.position.x, dotObject1.transform.position.y, dotObject1.transform.position.z - .065f)); // First dot
                lineMeasure.SetPosition(1, new Vector3(dotObject2.transform.position.x, dotObject2.transform.position.y, dotObject2.transform.position.z + .065f)); // Second dot

                // Optional: Set width
                lineMeasure.startWidth = 0.07f;
                lineMeasure.endWidth = 0.07f;

                Debug.Log("--> " + distance + " " + playerDecisionForDistance + " " + temp);

                yourDecisionObject.SetActive(true);
                yourDecisionDotObject.SetActive(true);
                lineMeasureObject.SetActive(true);
            }
            else
            {
                if (BallType == "Strike")
                {
                    decisionText.text = "Incorrect";
                    decisionTypeText.color = Color.red;
                }
                else
                {
                    decisionText.text = "Correct";
                    decisionTypeText.color = Color.green;
                }
                hitterActionText.text = "No Swing";

                decisionAccuracyText.text = "--";
                currentDecisionAccuracy = 0f;

                yourDecisionObject.SetActive(false);
                yourDecisionDotObject.SetActive(false);
                lineMeasureObject.gameObject.SetActive(false);
            }

        }

        if(swingTriggerResultHandler != null)
            swingTriggerResultHandler.expectedSwingString.Add(decisionText.text);

        if (currentDecisionAccuracy > maxDecisionAccuracy)
            maxDecisionAccuracy = currentDecisionAccuracy;

        Debug.Log($"Max Decision Accuracy: {maxDecisionAccuracy:F2}%");

        maxDecisionAccText.text = ConvertFeetToFeetInches(maxDecisionAccuracy);

        RatePitch();
        GridHighlight();
        isSwing = false;
        yourTime = 0;
    }

    public void AssignPitcherData()
    {
        pitcherName.text = DataPassOnUI.instance.pitcherDetails[pitchersSetting].pitcherName;

        pitcherName2.text = DataPassOnUI.instance.pitcherDetails[pitchersSetting].pitcherName;

        pitcherHeight.text = "Height : " + DataPassOnUI.instance.pitcherDetails[pitchersSetting].pitcherHeight;

        pitcherArm.text = "Arm : " + DataPassOnUI.instance.pitcherDetails[pitchersSetting].pitcherThrow;

        pitcherAngle.text = "Angle : " + DataPassOnUI.instance.pitcherDetails[pitchersSetting].pitcherArmslot;

        pitcherStyle.text = "Style : " + DataPassOnUI.instance.pitcherDetails[pitchersSetting].pitcherStyle;

        pitcherImage.sprite = DataPassOnUI.instance.pitcherDetails[pitchersSetting].pitcherSprite;
    }

    [ContextMenu("BaseballValueSetup_BallThrow")]
    public void BaseballValueSetup_BallThrow()
    {
        switch (pitchersSetting)
        {
            case 0:
                startTransform.position = Steve.position;
                break;
            case 1:
                startTransform.position = budSmith.position;
                break;
            case 2:
                startTransform.position = Dante.position;
                break;
            case 3:
                startTransform.position = Chris.position;
                break;
            case 4:
                startTransform.position = Brian.position;
                break;
            case 5:
                startTransform.position = Hiro.position;
                break;
            case 6:
                startTransform.position = Steve2.position;
                break;
        }

        RingIndicatorOnStart.transform.localPosition = new Vector3(startTransform.position.x, startTransform.position.y, startTransform.position.z);
        //RingIndicatorOnStart.SetActive(true);

        // endPoses = Random.Range(1, 31);
        switch (accuracySetting)
        {
            case 0: // 50/50 ball and strike options
                int i = Random.Range(0, 101);
                if (i <= 95)
                {
                    Vector3 newEndPoses;
                    int temp;
                    do
                    {
                        temp = Random.Range(0, Strikes.Count);
                        newEndPoses = Strikes[temp].transform.localPosition;
                    } while (endPosesHistory.Count >= MAX_CONSECUTIVE &&
                             endPosesHistory.TakeLast(MAX_CONSECUTIVE).All(pos => pos == newEndPoses));

                    endPoses = newEndPoses;
                    endPoseForline = values[Random.Range(0, values.Length)];
                    BallType = "Strike";
                    lineRendereMat.color = Color.green;

                    gridCounter = ExtractNumber(Strikes[temp].name);
                    Debug.Log($"this is end point {endPoses} of PinPoint " + "Ball" + temp);

                    UpdateEndPosesHistory(endPoses);
                }
                else
                {
                    Vector3 newEndPoses;
                    int temp;
                    do
                    {
                        temp = Random.Range(0, Strikes.Count);
                        newEndPoses = Balls[temp].transform.localPosition;
                    } while (endPosesHistory.Count >= MAX_CONSECUTIVE &&
                             endPosesHistory.TakeLast(MAX_CONSECUTIVE).All(pos => pos == newEndPoses));

                    endPoses = newEndPoses;
                    endPoseForline = values[Random.Range(0, values.Length)];
                    BallType = "Ball";
                    lineRendereMat.color = Color.red;

                    gridCounter = ExtractNumber(Strikes[temp].name);
                    Debug.Log($"this is end point {endPoses} of PinPoint " + "Strike" + temp);

                    UpdateEndPosesHistory(endPoses);
                }
                break;

            case 1:
                int j = Random.Range(0, 101);
                if (j <= 90)
                {
                    Vector3 newEndPoses;
                    int temp;
                    do
                    {
                        temp = Random.Range(0, Strikes.Count);
                        newEndPoses = Strikes[temp].transform.localPosition;
                    } while (endPosesHistory.Count >= MAX_CONSECUTIVE &&
                             endPosesHistory.TakeLast(MAX_CONSECUTIVE).All(pos => pos == newEndPoses));

                    endPoses = newEndPoses;
                    endPoseForline = values[Random.Range(0, values.Length)];
                    BallType = "Strike";
                    lineRendereMat.color = Color.green;

                    gridCounter = ExtractNumber(Strikes[temp].name);
                    Debug.Log($"this is end point {endPoses} of Accurate " + "Strike" + temp);

                    UpdateEndPosesHistory(endPoses);
                }
                else
                {
                    Vector3 newEndPoses;
                    int temp;
                    do
                    {
                        temp = Random.Range(0, Balls.Count);
                        newEndPoses = Balls[temp].transform.localPosition;
                    } while (endPosesHistory.Count >= MAX_CONSECUTIVE &&
                             endPosesHistory.TakeLast(MAX_CONSECUTIVE).All(pos => pos == newEndPoses));

                    endPoses = newEndPoses;
                    endPoseForline = values[Random.Range(0, values.Length)];
                    BallType = "Ball";
                    lineRendereMat.color = Color.red;

                    gridCounter = ExtractNumber(Balls[temp].name);
                    Debug.Log($"this is end point {endPoses} of Accurate " + "Ball" + temp);

                    UpdateEndPosesHistory(endPoses);
                }
                break;

            case 2:
                int x = Random.Range(0, 101);
                if (x <= 80)
                {
                    Vector3 newEndPoses;
                    int temp;
                    do
                    {
                        temp = Random.Range(0, Strikes.Count);
                        newEndPoses = Strikes[temp].transform.localPosition;
                    } while (endPosesHistory.Count >= MAX_CONSECUTIVE &&
                             endPosesHistory.TakeLast(MAX_CONSECUTIVE).All(pos => pos == newEndPoses));

                    endPoses = newEndPoses;
                    endPoseForline = values[Random.Range(0, values.Length)];
                    BallType = "Strike";
                    lineRendereMat.color = Color.green;

                    gridCounter = ExtractNumber(Strikes[temp].name);
                    Debug.Log($"this is end point {endPoses} of Finding the zone " + "Strike" + temp);

                    UpdateEndPosesHistory(endPoses);
                }
                else
                {
                    Vector3 newEndPoses;
                    int temp;
                    do
                    {
                        temp = Random.Range(0, Balls.Count);
                        newEndPoses = Balls[temp].transform.localPosition;
                    } while (endPosesHistory.Count >= MAX_CONSECUTIVE &&
                             endPosesHistory.TakeLast(MAX_CONSECUTIVE).All(pos => pos == newEndPoses));

                    endPoses = newEndPoses;
                    endPoseForline = values[Random.Range(0, values.Length)];
                    BallType = "Ball";
                    lineRendereMat.color = Color.red;

                    gridCounter = ExtractNumber(Balls[temp].name);
                    Debug.Log($"this is end point {endPoses} of Finding the zone " + "Ball" + temp);

                    UpdateEndPosesHistory(endPoses);
                }
                break;

            case 3:
                int y = Random.Range(0, 101);
                if (y <= 50)
                {
                    Vector3 newEndPoses;
                    int temp;
                    do
                    {
                        temp = Random.Range(0, Strikes.Count);
                        newEndPoses = Strikes[temp].transform.localPosition;
                    } while (endPosesHistory.Count >= MAX_CONSECUTIVE &&
                             endPosesHistory.TakeLast(MAX_CONSECUTIVE).All(pos => pos == newEndPoses));

                    endPoses = newEndPoses;
                    endPoseForline = values[Random.Range(0, values.Length)];
                    BallType = "Strike";
                    lineRendereMat.color = Color.green;

                    gridCounter = ExtractNumber(Strikes[temp].name);
                    Debug.Log($"this is end point {endPoses} of PinPointButton " + "Strike" + temp);

                    UpdateEndPosesHistory(endPoses);
                }
                else
                {
                    Vector3 newEndPoses;
                    int temp;
                    do
                    {
                        temp = Random.Range(0, Balls.Count);
                        newEndPoses = Balls[temp].transform.localPosition;
                    } while (endPosesHistory.Count >= MAX_CONSECUTIVE &&
                             endPosesHistory.TakeLast(MAX_CONSECUTIVE).All(pos => pos == newEndPoses));

                    endPoses = newEndPoses;
                    endPoseForline = values[Random.Range(0, values.Length)];
                    BallType = "Ball";
                    lineRendereMat.color = Color.red;

                    gridCounter = ExtractNumber(Balls[temp].name);
                    Debug.Log($"this is end point {endPoses} of PinPointButton " + "Ball" + temp);

                    UpdateEndPosesHistory(endPoses);
                }
                break;
        }

        tweenTimer = 0;
        isTweening = true;
        isStartTime = false;
        // Prevent same pitch type after two consecutive occurrences

        //List<int> availablePitchTypes = new List<int> { 0, 1, 2, 3 }; // Fastball, Curveball, Slider, Changeup

        //if (lastTwoPitches.Count >= 2 && lastTwoPitches[lastTwoPitches.Count - 1] == lastTwoPitches[lastTwoPitches.Count - 2])
        //{
        //    string lastPitchType = lastTwoPitches[lastTwoPitches.Count - 1];
        //    int pitchIndexToRemove = -1;
        //    switch (lastPitchType)
        //    {
        //        case "Fastball": pitchIndexToRemove = 0; break;
        //        case "Curveball": pitchIndexToRemove = 1; break;
        //        case "Slider": pitchIndexToRemove = 2; break;
        //        case "Changeup": pitchIndexToRemove = 3; break;
        //    }

        //    if (pitchIndexToRemove != -1)
        //    {
        //        availablePitchTypes.Remove(pitchIndexToRemove);
        //    }
        //}

        //pitchTypes = availablePitchTypes[Random.Range(0, availablePitchTypes.Count)];

        List<int> allPitchTypes = new List<int> { 0, 1, 2, 3 }; // Fastball = 0, ..., Changeup = 3

        // 1. Prevent repeating same pitch twice
        if (lastTwoPitches.Count >= 2 && lastTwoPitches[^1] == lastTwoPitches[^2])
        {
            string lastPitchType = lastTwoPitches[^1];
            int pitchIndexToRemove = lastPitchType switch
            {
                "Fastball" => 0,
                "Curveball" => 1,
                "Slider" => 2,
                "Changeup" => 3,
                _ => -1
            };

            if (pitchIndexToRemove != -1)
            {
                allPitchTypes.Remove(pitchIndexToRemove);
            }
        }

        // 2. Define weighted pitch list (Fastball: 50%, Others: 16.66% each)
        List<int> weightedPitchPool = new List<int>();
        foreach (int type in allPitchTypes)
        {
            int weight = (type == 0) ? 50 : 17; // Use 17 to slightly overweight others (adjustable)
            for (int i = 0; i < weight; i++)
            {
                weightedPitchPool.Add(type);
            }
        }

        // 3. Randomly select pitch based on weights
        pitchTypes = weightedPitchPool[Random.Range(0, weightedPitchPool.Count)];

        Vector3 handleTransformlocalPosition;
        if (isTweening)
        {
            switch (pitchTypes)
            {
                case 0:
                    pitchType = "Fastball";
                    //handleTransformlocalPosition = handleDefault.gameObject.GetComponent<TransformHandlerForPitch>().transformHandler[velocitySetting];

                    handleTransform.localPosition = handleDefault.localPosition;
                    //+ new Vector3(
                    //    Random.Range(-0.1f, 0.1f), // Random x offset
                    //    Random.Range(-0.1f, 0.1f), // Random y offset
                    //    0f); // Keep z unchanged
                    lastTwoPitches.Add(pitchType);
                    Debug.Log("FastBall");
                    break;

                case 1:
                    pitchType = "Curveball";
                    if (pitchersSetting == 1)
                    {
                        handleTransformlocalPosition = handleDefaultCurve[0].gameObject.GetComponent<TransformHandlerForPitch>().transformHandler[velocitySetting];

                        handleTransform.localPosition = handleTransformlocalPosition + new Vector3(
                Random.Range(-0.1f, 0.1f), // Random x offset
                Random.Range(-0.1f, 0.1f), // Random y offset
                0f);

                    }
                    else
                    {
                        handleTransformlocalPosition = handleDefaultCurve[1].gameObject.GetComponent<TransformHandlerForPitch>().transformHandler[velocitySetting];

                        handleTransform.localPosition = handleTransformlocalPosition + new Vector3(
                Random.Range(-0.1f, 0.1f), // Random x offset
                Random.Range(-0.1f, 0.1f), // Random y offset
                0f);
                    }
                    lastTwoPitches.Add(pitchType);
                    Debug.Log("Curveball");
                    break;

                case 2:
                    pitchType = "Slider";
                    if (pitchersSetting == 1)
                    {
                        handleTransformlocalPosition = handleDefaultSlider[0].gameObject.GetComponent<TransformHandlerForPitch>().transformHandler[velocitySetting];
                        handleTransform.localPosition = handleTransformlocalPosition + new Vector3(
                Random.Range(-0.1f, 0.1f), // Random x offset
                Random.Range(-0.1f, 0.1f), // Random y offset
                0f);
                    }
                    else
                    {
                        handleTransformlocalPosition = handleDefaultSlider[1].gameObject.GetComponent<TransformHandlerForPitch>().transformHandler[velocitySetting];
                        handleTransform.localPosition = handleTransformlocalPosition + new Vector3(
                Random.Range(-0.1f, 0.1f), // Random x offset
                Random.Range(-0.1f, 0.1f), // Random y offset
                0f);
                    }
                    lastTwoPitches.Add(pitchType);
                    Debug.Log("Slider");
                    break;

                case 3:
                    pitchType = "Changeup";
                    if (pitchersSetting == 1)
                    {
                        handleTransformlocalPosition = handleDefaultChangeUp[0].gameObject.GetComponent<TransformHandlerForPitch>().transformHandler[velocitySetting];
                        handleTransform.localPosition = handleTransformlocalPosition + new Vector3(
                Random.Range(-0.1f, 0.1f), // Random x offset
                Random.Range(-0.1f, 0.1f), // Random y offset
                0f);
                    }
                    else
                    {
                        handleTransformlocalPosition = handleDefaultChangeUp[1].gameObject.GetComponent<TransformHandlerForPitch>().transformHandler[velocitySetting];
                        handleTransform.localPosition = handleTransformlocalPosition + new Vector3(
                Random.Range(-0.1f, 0.1f), // Random x offset
                Random.Range(-0.1f, 0.1f), // Random y offset
                0f);
                    }
                    lastTwoPitches.Add(pitchType);
                    Debug.Log("ChangeUp");
                    break;
            }

            // Keep only the last two pitch types

            if (lastTwoPitches.Count > 2)
            {
                lastTwoPitches.RemoveAt(0);

            }

            endTransform.localPosition = endPoses;
        }

        Debug.Log($"this is endpoint {endPoses} and pitch type {pitchType}" + endTransform.position + "--" + endTransform.localPosition);
        Debug.Log($"this is endPoseForline {endPoseForline}");
    }

    private void UpdateEndPosesHistory(Vector3 newEndPoses)
    {
        endPosesHistory.Add(newEndPoses);
        if (endPosesHistory.Count > MAX_CONSECUTIVE)
        {
            endPosesHistory.RemoveAt(0); // Keep only the last two endpoints
        }
    }

    public int ExtractNumber(string input)
    {
        string[] parts = input.Split(' '); // Split by space

        string numberString = parts[parts.Length - 1]; // Get the last part
        int number = int.Parse(numberString); // Convert to integer

        Debug.Log("Extracted Number: " + number);
        return number;
    }

    private Vector3 CalcPositionOnCurve(float percent)
    {
        Vector3 c = AnimMath.Lerp(startTransform.position, handleTransform.position, percent);
        Vector3 d = AnimMath.Lerp(handleTransform.position, endTransform.position, percent);

        Vector3 f = AnimMath.Lerp(c, d, percent);

        return f;
    }

    private Vector3 CalcPositionOnCurveForLine(float percent)
    {
        Vector3 c = AnimMath.Lerp(startTransform.position, handleTransform.position, percent);
        Vector3 d = AnimMath.Lerp(handleTransform.position, endTransform.position, percent);

        Vector3 f = AnimMath.Lerp(c, d, percent);

        //if (percent == 0) f += (f - d).normalized * .2f; // Extend start
        //if (percent == 1) f += (f - c).normalized * .2f; // Extend end
        //Vector3 straightDirection = (endTransform.position - startTransform.position).normalized;
        //if (percent == 0)
        //{
        //    f += straightDirection * 0.2f; // Extend along straight line direction
        //}
        //// Extend end
        //else if (percent == 1)
        //{
        //    f += straightDirection * 0.2f; // Extend along straight line direction
        //}

        return f;
    }

    //private Vector3 CalcPositionOnCurveForLine(float percent)
    //{
    //    // Quadratic Bézier curve for the main path
    //    Vector3 c = AnimMath.Lerp(startTransform.position, handleTransform.position, percent);
    //    Vector3 d = AnimMath.Lerp(handleTransform.position, endTransform.position, percent);
    //    Vector3 f = AnimMath.Lerp(c, d, percent);

    //    // Direction for straight extensions (from start to end)
    //    Vector3 straightDirection = (endTransform.position - startTransform.position).normalized;

    //    // Extend start
    //    if (percent == 0)
    //    {
    //        f += straightDirection * 0.2f; // Extend along straight line direction
    //    }
    //    // Extend end
    //    else if (percent == 1)
    //    {
    //        f += straightDirection * 0.2f; // Extend along straight line direction
    //    }

    //    return f;
    //}

    private Vector3 CalcPositionOnCurveChangeUP(float percent)
    {
        Vector3 c = AnimMath.Lerp(startTransform.position, handleTransform.position, percent);
        Vector3 d = AnimMath.Lerp(handleTransform.position, handleEndTransform.position, percent);
        Vector3 e = AnimMath.Lerp(handleEndTransform.position, endTransform.position, percent);

        Vector3 f = AnimMath.Lerp(c, d, percent);
        Vector3 g = AnimMath.Lerp(d, e, percent);
        Vector3 h = AnimMath.Lerp(f, g, percent);

        return h;
    }

    private Vector3 CalcPositionOnCurveChangeUPForLine(float percent)
    {
        Vector3 c = AnimMath.Lerp(startTransform.position, handleTransform.position, percent);
        Vector3 d = AnimMath.Lerp(handleTransform.position, handleEndTransform.position, percent);
        Vector3 e = AnimMath.Lerp(handleEndTransform.position, endTransform.position, percent);

        Vector3 f = AnimMath.Lerp(c, d, percent);
        Vector3 g = AnimMath.Lerp(d, e, percent);
        Vector3 h = AnimMath.Lerp(f, g, percent);

        if (percent == 0) f += (f - d).normalized * .2f; // Extend start
        if (percent == 1) f += (f - c).normalized * .2f; // Extend end

        return h;
    }

    private void OnDrawGizmos()
    {
        Vector3 p1 = startTransform.position;
        for (int i = 1; i < curveResolution; i++)
        {
            float p = i / (float)curveResolution;
            Vector3 p2;//= CalcPositionOnCurveForLine(p);

            //if (pitchTypes == 3 || pitchTypes == 2)
            //{
            //p2 = CalcPositionOnCurveChangeUPForLine(p);
            //}
            //else
            //{
            p2 = CalcPositionOnCurveForLine(p);
            //}

            Gizmos.DrawLine(p1, p2);
            p1 = p2;
        }
        Gizmos.DrawLine(p1, endTransform.position);
    }

    void UpdateTrajectoryPath()
    {
        if (lineRenderer == null) return;

        int resolution = 50; // Increase for smoother curve
        Vector3[] positions = new Vector3[resolution + 1];

        for (int i = 0; i <= resolution; i++)
        {
            float percent = i / (float)resolution;

            //if (pitchTypes == 3 || pitchTypes == 2)
            //{
            //    positions[i] = CalcPositionOnCurveChangeUPForLine(percent);
            //}
            //else
            {
                positions[i] = CalcPositionOnCurveForLine(percent);
            }
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
        Vector3 midPosition = Vector3.zero;
        Vector3 optimalPosition = Vector3.zero;
        bool foundZ6 = false;
        bool foundZ2 = false;

        if (positions.Length > 1)
        {
            int pointCount = lineRenderer.positionCount;

            for (int i = 0; i < pointCount; i++)
            {
                Vector3 point = lineRenderer.GetPosition(i);
                if (Mathf.FloorToInt(point.z) < -6)
                {
                    Debug.Log("--> Found point with Z = 6: " + point);
                    midPosition = point;
                    foundZ6 = true;
                    break;
                }
            }

            for (int i = 0; i < pointCount; i++)
            {
                Vector3 point = lineRenderer.GetPosition(i);
                if (Mathf.FloorToInt(point.z) < 3)
                {
                    Debug.Log("--> Found point with Z = 6: " + point);
                    optimalPosition = point;
                    foundZ2 = true;
                    break;
                }
            }

            int midIndex = 0;

            if (!foundZ6)
            {
                midIndex = positions.Length / 2;
                midIndex = Mathf.Clamp(midIndex, 0, positions.Length - 1);

                midPosition = positions[midIndex];
            }

            int optimalIndex = 0;

            if (!foundZ2)
            {
                optimalIndex = positions.Length / 2;
                optimalIndex = Mathf.Clamp(optimalIndex, 0, positions.Length - 1);

                optimalPosition = positions[optimalIndex];
            }

            movingObject.transform.localPosition = midPosition;
            RingIndicatorOnStart.transform.localPosition = optimalPosition;

            Debug.Log($"Mid Index: {midIndex}, Positions Length: {positions.Length}, Pos: {midPosition}");
        }

        if (!isVirtualDrill)
        {
            if (isSwing)
                RingIndicatorOnTrigger.SetActive(true);

            RingIndicatorOnStart.SetActive(true);
        }
    }

    public void AssignVelocity()
    {
        velocitySetting = DataPassOnUI.instance.velocitySetting;
    }

    public void AssignAccuracy()
    {
        accuracySetting = DataPassOnUI.instance.accuracySetting;
    }

    public void AssignPitcher()
    {
        pitchersSetting = DataPassOnUI.instance.pitchersSetting;

        AssignPitcherData();
    }

    // Accuracy percentage
    void CalculateTimingAccuracy()
    {
        float timeDiff;
        if (swingStartTime < decisionMinValue)
        {
            timeDiff = Mathf.Abs(swingStartTime - decisionMinValue);  // Too Early
        }
        else if (swingStartTime > decisionMaxValue)
        {
            timeDiff = Mathf.Abs(swingStartTime - decisionMaxValue);  // Too Late
        }
        else
        {
            timeDiff = 0;  // Perfect timing
        }

        float timingAccuracy = Mathf.Clamp(100 - (timeDiff * 500), 0, 100);  // Adjust sensitivity
        Debug.Log("Swing Accuracy: " + timingAccuracy + "%");
    }

    float CalculateVelocityScore(float pitchSpeed, string pitchType)
    {
        float minSpeed = 0, maxSpeed = 0;

        switch (pitchType)
        {
            case "Fastball": minSpeed = minFastSpeed; maxSpeed = maxFastSpeed; break;  // Example range
            case "Slider": minSpeed = minSliderSpeed; maxSpeed = maxSliderSpeed; break;
            case "Curveball": minSpeed = minCurveBallSpeed; maxSpeed = maxCurveBallSpeed; break;
            case "Changeup": minSpeed = minChangeupSpeed; maxSpeed = maxChangeupSpeed; break;
        }

        // If pitchSpeed is within the range, it's perfect
        if (pitchSpeed >= minSpeed && pitchSpeed <= maxSpeed)
        {
            return 100; // Perfect score
        }

        // If it's outside, calculate accuracy based on the closest boundary
        float speedDiff = Mathf.Min(Mathf.Abs(pitchSpeed - minSpeed), Mathf.Abs(pitchSpeed - maxSpeed));
        return Mathf.Clamp(100 - (speedDiff * 5), 50, 100);  // Base score between 50-100
    }

    float CalculateEffectivenessScore(bool isStrike, bool isSwing)
    {
        if (isStrike && !isSwing) return 100; // Called Strike
        if (isStrike && isSwing) return 70; // Hit Ball (Foul or Play)
        if (!isStrike && !isSwing) return 80; // Good Ball Placement
        return 50; // Poor pitch
    }

    float CalculateAccuracyScore(int accuracySetting)
    {
        float idealAccuracy = 0;
        switch (accuracySetting)
        {
            case 0: idealAccuracy = 95f; break;
            case 1: idealAccuracy = 90f; break;
            case 2: idealAccuracy = 80f; break;
            case 3: idealAccuracy = 50f; break;
        }

        return idealAccuracy;
    }

    public void RatePitch()
    {
        velocityScore = CalculateVelocityScore(baseballSpeedMPH, pitchType);
        effectivenessScore = CalculateEffectivenessScore(BallType == "Strike", isSwing);
        accuracyScore = CalculateAccuracyScore(accuracySetting);

        finalRating = (velocityScore * 0.4f) + (accuracyScore * 0.3f) + (effectivenessScore * 0.3f);

        Debug.Log($"Pitch Rating: {finalRating}");

        UpdateAccuracyTracking(finalRating);

        float mappedValue = 90 - finalRating * 1.8f;
        Debug.Log("Mapped Value: " + mappedValue);

        // Apply Rotation to a UI Arrow or Meter
        pitchRatingArrow.transform.localRotation = Quaternion.Euler(0, 0, mappedValue);
    }

    private float maxBallDistance = 0f;
    private float maxTravelTime = 0f;
    void BallDistanceAndTimeCalcultion()
    {
        //float decisionTime = (optimalTime / 3.33f);
        //decisionTime = (float)Math.Round(decisionTime, 3);
        Debug.Log("--> Ball time : " + optimalTime + "--> Ball Decision : " + yourTime);

        if (yourTime > 0)
        {
            decisionTypeText.gameObject.SetActive(true);
            if (yourTime < optimalTime)
            {
                Debug.Log("--> Decision Time was TOO EARLY! 🚨");
                decisionTypeText.text = ""; //"Too Early";
            }
            else if (yourTime > optimalTime)
            {
                Debug.Log("--> Decision Time was TOO LATE! ❌");
                decisionTypeText.text = "Too Late";
            }
            else
            {
                Debug.Log("--> Decision Time was ON TIME! ✅");
                decisionTypeText.text = "On Time";
            }
        }

        Vector3 newEndPos = new Vector3(endTransform.position.x, endTransform.position.y, endTransform.position.z + endPoseForline);
        float distance = CalculateBallDistance(startTransform.position, newEndPos);

        //float speedMps = (distance/ optimalTime) * 1.5f;
        float travelTime = optimalTime;

        //baseballSpeedMPH = speedMps;
        switch (pitchType)
        {
            case "Fastball":
                baseballSpeedMPH = currentFastSpeed;
                break;
            case "Slider":
                baseballSpeedMPH = currentSliderSpeed;
                break;
            case "Curveball":
                baseballSpeedMPH = currentCurveBallSpeed;
                break;
            case "Changeup":
                baseballSpeedMPH = currentChangeupSpeed;
                break;
            default:
                Debug.LogWarning("Unknown pitch type! Assigning default speed.");
                baseballSpeedMPH = 80f; // Default fallback speed
                break;
        }

        // Update Max Values
        if (distance > maxBallDistance)
            maxBallDistance = distance;

        if (travelTime > maxTravelTime)
            maxTravelTime = travelTime;

        Debug.Log($"--> Ball Distance: {distance:F2}m, Travel Time: {travelTime:F3}s, Speed MPS: {baseballSpeedMPH:F2}");
        Debug.Log($"Max Distance: {maxBallDistance:F2}m, Max Time: {maxTravelTime:F2}s");

        // Update UI Text
        ballDistanceMeterText.text = ConvertFeetToFeetInches(distance);
        ballDistnaceTimeText.text = travelTime.ToString("F3") + "s";

        maxBallDistanceText.text = ConvertFeetToFeetInches(maxBallDistance);
        maxTravelTimeText.text = maxTravelTime.ToString("F3") + "s";
    }

    float CalculateBallDistance(Vector3 start, Vector3 end)
    {
        return Vector3.Distance(start, end);
    }

    public void UpdateStrikeTracking()
    {
        bool isStrike = (BallType == "Strike");
        last10Throws.Add(isStrike);

        totalPitches++;
        if (isStrike) totalStrikes++;

        // Calculate Strike % (out of last 10 throws)
        int recentStrikes = last10Throws.FindAll(x => x == true).Count;
        strikeAverage = (recentStrikes / 10f) * 100f;

        Debug.Log($"Total Strikes: {totalStrikes}, Strike % in Last 10: {strikeAverage:F2}%");

        pitchTypeAvgText.text = strikeAverage.ToString("F2") + "%";
        pitchTypeAvgFontText.text = strikeAverage.ToString("F2") + "%";
    }

    public void UpdateAccuracyTracking(float accuracyScore)
    {
        last10Accuracies.Add(accuracyScore);

        // Calculate Average Accuracy over last 10 pitches
        float totalAccuracy = 0;
        foreach (float accuracy in last10Accuracies)
        {
            totalAccuracy += accuracy;
        }

        averageAccuracy = last10Accuracies.Count > 0 ? totalAccuracy / last10Accuracies.Count : 0;

        float maxAccuracy = last10Accuracies.Count > 0 ? Mathf.Max(last10Accuracies.ToArray()) : 0;

        Debug.Log($"Max Accuracy (Last 10 Pitches): {maxAccuracy:F2}%");

        Debug.Log($"Average Accuracy (Last 10 Pitches): {averageAccuracy:F2}%");
        accuracyAvgText.text = averageAccuracy.ToString("F2") + "%";

        maxAccuracyAvgText.text = maxAccuracy.ToString("F2") + "%";

        Debug.Log("round rating arrow : " + roundRatingArrow.GetComponent<RectTransform>().localPosition.x + "--" + (int)averageAccuracy * 4.7f);

        roundRatingArrow.GetComponent<RectTransform>().localPosition = new Vector3( (int)averageAccuracy * 4.7f - 245,
            (roundRatingArrow.GetComponent<RectTransform>().localPosition.y), (roundRatingArrow.GetComponent<RectTransform>().localPosition.z));
    }

    string ConvertFeetToFeetInches(float value)
    {
        int feet = Mathf.FloorToInt(value); // Extract whole feet
        int inches = Mathf.RoundToInt((value - feet) * 12); // Convert decimal feet to inches

        return $"{feet}'{inches}\""; // Format as Feet'Inches"
    }

    public void ResetValue()
    {
        // Reset game state variables


        RingIndicatorOnTrigger.SetActive(false);
        RingIndicatorOnStart.SetActive(false);
        decisionTypeText.gameObject.SetActive(false);

        isSwing = false;
        isStartTime = false;
        isTweening = false;
        isEndLoop = false;
        tweenTimer = 0;
        percent = 0;
        baseballSpeedMPH = 0;
        finalRating = 0;
        speedMultiplier = 0;
        pitchType = "";
        BallType = "";
        totalPitches = 0;
        totalStrikes = 0;
        strikeAverage = 0;
        averageAccuracy = 0;
        maxDecisionAccuracy = 0;
        maxBallDistance = 0;
        maxTravelTime = 0;

        // Clear lists
        last10Throws.Clear();
        last10Accuracies.Clear();
        lastTwoPitches.Clear(); // Clear pitch history
        endPosesHistory.Clear();

        // Reset UI elements
        speedOMeterText.text = "-- MPH";
        recognitionWindowText.text = "Recognition Window";
        pitchCalledText.text = "";
        hitterActionText.text = "";
        decisionText.text = "";
        decisionAccuracyText.text = "--";
        ballDistanceMeterText.text = "0.00m";
        ballDistnaceTimeText.text = "0.00s";
        pitchTypeText.text = "";
        ballSpeedText.text = "0.00 MPH";
        windowRecogText.text = "Recognition Window";
        pitchTypeAvgText.text = "0.00%";
        pitchTypeAvgFontText.text = "0.00%";
        accuracyAvgText.text = "0.00%";
        maxBallDistanceText.text = "0.00m";
        maxTravelTimeText.text = "0.00s";
        maxAccuracyAvgText.text = "0.00%";
        maxDecisionAccText.text = "0.00%";
        optimalTime = 0f;
        yourTime = 0f;


        // Reset UI arrows
        pitchRatingArrow.transform.localRotation = Quaternion.Euler(0, 0, 90);
        roundRatingArrow.GetComponent<RectTransform>().localPosition = new Vector3(0, roundRatingArrow.GetComponent<RectTransform>().localPosition.y, roundRatingArrow.GetComponent<RectTransform>().localPosition.z);

        // Hide UI elements if necessary
        speedOMeterObject.SetActive(false);


        Debug.Log("All values reset.");
    }

    public void GridHighlight()
    {
        for (int i = 0; i < gridHighlight.Length; i++)
        {
            if (i == gridCounter - 1)
            {
                gridHighlight[i].SetActive(true);
            }
            else
            {
                gridHighlight[i].SetActive(false);
            }
        }
    }
}
