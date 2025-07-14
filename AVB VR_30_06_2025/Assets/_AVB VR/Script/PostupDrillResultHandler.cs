using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PostupDrillResultHandler : MonoBehaviour
{
    [Header("For Result")]
    public List<string> BallTypeString;
    public List<string> isSwingString;
    public List<string> ballResult;
    public GameObject ballTypePrefab;
    public GameObject swingPrefab;
    public GameObject CorrectAnsPrefab;
    public GameObject IncorrectAnsPrefab;

    [Header("UI")]
    public GameObject containerBallType;
    public GameObject containerIsSwing;
    public GameObject containerResult;
    public GameObject resultPanel;

    public TextMeshProUGUI CorrectAnsText;
    public TextMeshProUGUI DecisionText;
    public string currentAnsString;

    [Header("Trajectory")]
    public GameObject ballLineTrajectory;
    public GameObject ball;
    public GameObject ringIndicatorOnTrigger;
    public GameObject ringIndicatorOnStart;

    public void CheckAnswer()
    {
        if (DecisionText.text == "Correct")
        {
            DecisionText.color = Color.green;
        }
        else
        {
            DecisionText.color = Color.red;
        }

        CorrectAnsText.text = "Currect Answer : " + currentAnsString;
        CorrectAnsText.gameObject.SetActive(true);

        ballLineTrajectory.SetActive(true);
        ball.SetActive(true);
        ringIndicatorOnTrigger.SetActive(true);
        ringIndicatorOnStart.SetActive(true);
    }

    public void DisplayResults()
    {
        for (int i = 0; i < ballResult.Count; i++)
        {
            // Instantiate correct answer prefab (green/red ball)
            GameObject ballTypeObj = Instantiate(ballTypePrefab, containerBallType.transform);
            ballTypeObj.GetComponent<TextMeshProUGUI>().text = BallTypeString[i];

            // Instantiate your answer prefab (green/red hand)
            GameObject swingObj = Instantiate(swingPrefab, containerIsSwing.transform);
            swingObj.GetComponent<TextMeshProUGUI>().text = isSwingString[i];

            // Instantiate result prefab (tick/cross)

            bool isCorrect = (ballResult[i] == "Correct" ? true : false);

            GameObject resultObj = Instantiate(isCorrect ? CorrectAnsPrefab : IncorrectAnsPrefab, containerResult.transform);
        }
        resultPanel.SetActive(true);
    }

    public void ResetResult()
    {
        // Clear UI containers
        foreach (Transform child in containerBallType.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in containerIsSwing.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in containerResult.transform)
        {
            Destroy(child.gameObject);
        }

        // Optional: clear data lists
        BallTypeString.Clear();
        isSwingString.Clear();
        ballResult.Clear();

        // Hide result panel
        resultPanel.SetActive(false);
    }
}
