using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SwingTriggerResultHandler : MonoBehaviour
{
    [Header("For Result")]
    public List<string> BallTypeString;
    public List<string> expectedSwingString;
    public List<string> decisionTime;
    public GameObject ballTypePrefab;
    public GameObject decsionTimePrefab;
    public GameObject CorrectSwingAnsPrefab;
    public GameObject IncorrectSwingAnsPrefab;

    [Header("UI")]
    public GameObject containerBallType;
    public GameObject containerIsSwing;
    public GameObject containerDecisionTime;
    public GameObject resultPanel;

    public void DisplayResults()
    {
        for (int i = 0; i < BallTypeString.Count; i++)
        {
            // Instantiate correct answer prefab (green/red ball)
            GameObject ballTypeObj = Instantiate(ballTypePrefab, containerBallType.transform);
            ballTypeObj.GetComponent<TextMeshProUGUI>().text = BallTypeString[i];

            // Instantiate your answer prefab (green/red hand)
            GameObject decisionTimeObj = Instantiate(decsionTimePrefab, containerDecisionTime.transform);
            decisionTimeObj.GetComponent<TextMeshProUGUI>().text = decisionTime[i];

            // Instantiate result prefab (tick/cross)

            bool isCorrect = (expectedSwingString[i] == "Correct" ? true : false);

            GameObject resultObj = Instantiate(isCorrect ? CorrectSwingAnsPrefab : IncorrectSwingAnsPrefab, containerIsSwing.transform);
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

        foreach (Transform child in containerDecisionTime.transform)
        {
            Destroy(child.gameObject);
        }

        // Optional: clear data lists
        BallTypeString.Clear();
        expectedSwingString.Clear();
        decisionTime.Clear();

        // Hide result panel
        resultPanel.SetActive(false);
    }
}
