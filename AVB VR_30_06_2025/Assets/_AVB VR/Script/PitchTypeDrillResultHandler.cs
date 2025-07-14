using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PitchTypeDrillResultHandler : MonoBehaviour
{
    public Button[] answerButton;
    public TextMeshProUGUI CorrectAnsText;
    public TextMeshProUGUI DecisionText;
    public string currentAnsString;
    public string currentExpectedAnsString;

    [Header("For Result")]
    public List<string> ExpectedResult;
    public List<string> YourResult;
    public GameObject YourAnsPrefabTrue;
    public GameObject YourAnsPrefabFalse;
    public GameObject CorrectAnsPrefab;
    public GameObject containerCorrectAnswer;
    public GameObject containerYouranswer;
    public GameObject resultPanel;

    [Header("Trajectory")]
    public GameObject ballLineTrajectory;
    public GameObject ball;


    public void Assign_BallName(string BallName)
    {
        currentAnsString = BallName;
        YourResult.Add(BallName);
        CheckAnswer();


        ballLineTrajectory.SetActive(true);
        ball.SetActive(true);
    }

    public void CheckAnswer()
    {
        if (currentAnsString == currentExpectedAnsString)
        {
            DecisionText.text = "Correct";
            DecisionText.color = Color.green;
        }
        else
        {
            DecisionText.text = "Incorrect";
            DecisionText.color = Color.red;
        }

        CorrectAnsText.text = "Currect Answer : " + currentExpectedAnsString;
        CorrectAnsText.gameObject.SetActive(true);

        for (int i = 0; i < answerButton.Length; i++)
        {
            answerButton[i].GetComponent<Button>().interactable = false;
            answerButton[i].GetComponent<Image>().raycastTarget = false;
        }
    }

    public void DisplayResults()
    {
        for (int i = 0; i < YourResult.Count; i++)
        {
            // Instantiate correct answer prefab (green/red ball)
            GameObject correctAnsObj = Instantiate(CorrectAnsPrefab, containerCorrectAnswer.transform);
            correctAnsObj.GetComponent<TextMeshProUGUI>().text = ExpectedResult[i];

            bool isCorrect = (ExpectedResult[i] == YourResult[i] ? true : false);

            GameObject resultObj = Instantiate(isCorrect ? YourAnsPrefabTrue : YourAnsPrefabFalse, containerYouranswer.transform);
            resultObj.GetComponent<TextMeshProUGUI>().text = YourResult[i];
        }

        CorrectAnsText.gameObject.SetActive(false);
        resultPanel.SetActive(true);
    }

    public void ButtonInteractable()
    {
        for (int i = 0; i < answerButton.Length; i++)
        {
            answerButton[i].GetComponent<Button>().interactable = true;
            answerButton[i].GetComponent<Image>().raycastTarget = true;
        }
    }

    public void ResetResult()
    {
        // Clear children from correct answer container
        foreach (Transform child in containerCorrectAnswer.transform)
        {
            Destroy(child.gameObject);
        }

        // Clear children from your answer container
        foreach (Transform child in containerYouranswer.transform)
        {
            Destroy(child.gameObject);
        }

        // Clear result data
        ExpectedResult.Clear();
        YourResult.Clear();

        // Reset UI text if needed
        CorrectAnsText.text = "";
        CorrectAnsText.gameObject.SetActive(false);
        resultPanel.SetActive(false);

        // Re-enable answer buttons
        foreach (Button btn in answerButton)
        {
            btn.interactable = true;
        }
    }

}
