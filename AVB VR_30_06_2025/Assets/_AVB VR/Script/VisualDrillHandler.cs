using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class VisualDrillHandler : MonoBehaviour
{
    public static VisualDrillHandler instance;

    public GameObject[] cSpriteForBall;
    public GameObject[] cButton;
    public GameObject cSelectionPanel;
    public string currentCBallValue;
    public string currentCUIValue;
    public GameObject[] cBallResultImg;
    public TextMeshProUGUI CorrectAns;
    public TextMeshProUGUI DecisionText;
    public string ballType;

    [Header("For Result")]
    public List<string> cBallExpectedResult;
    public List<string> cBallYourResult;
    public GameObject[] cYourAnsPrefabTrue;
    public GameObject[] cYourAnsPrefabFalse;
    public GameObject[] cCorrectAnsPrefab;
    public GameObject containerCorrectAnswer;
    public GameObject containerYouranswer;
    public GameObject resultPanel;



    private void Awake()
    {
        // Ensure only one instance of DataPassOnUI exists
        if (instance != null)
        {
            Destroy(gameObject); // Destroy the duplicate instance
            return;
        }

        instance = this; // Set the current instance
    }

    public void Assign_BallName(string cBallName)
    {
        currentCUIValue = cBallName;

        cBallYourResult.Add(currentCUIValue);
        Check_Answer();
    }

    public void Check_Answer()
    {
        if (cSpriteForBall.Length != 0)
        {
            ballType = "Ball";

            if (currentCUIValue == currentCBallValue)
            {
                DecisionText.text = "Correct";
                DecisionText.color = Color.green;
            }
            else
            {
                DecisionText.text = "Incorrect";
                DecisionText.color = Color.red;
            }

            CorrectAns.text = "Currect Answer : ";
            for (int i = 0; i < cBallResultImg.Length; i++)
            {
                if (cBallResultImg[i].name == currentCBallValue)
                {
                    cBallResultImg[i].SetActive(true);
                }
                else
                {
                    cBallResultImg[i].SetActive(false);
                }
            }

            CorrectAns.gameObject.SetActive(true);
            for (int i = 0; i < cButton.Length; i++)
            {
                cButton[i].GetComponent<Button>().interactable = false;
                cButton[i].GetComponent<Image>().raycastTarget = false;
            }
        }
    }

    public void BallButtonInteractable()
    {
        for (int i = 0; i < cButton.Length; i++)
        {
            cButton[i].GetComponent<Button>().interactable = true;
            cButton[i].GetComponent<Image>().raycastTarget = true;
        }
    }

    public void AssignCShape_ToBall()
    {
        if (cSpriteForBall.Length != 0)
        {
            int index = Random.Range(0, cSpriteForBall.Length);
            for (int i = 0; i < cSpriteForBall.Length; i++)
            {
                cSpriteForBall[i].SetActive(false);
            }

            cSpriteForBall[index].SetActive(true);
            currentCBallValue = cSpriteForBall[index].name;
            cBallExpectedResult.Add(cSpriteForBall[index].name);
        }
    }

    public void DisableCShape_Ball()
    {
        int index = Random.Range(0, cSpriteForBall.Length);
        for (int i = 0; i < cSpriteForBall.Length; i++)
        {
            cSpriteForBall[i].SetActive(false);
        }
    }

    public void ResultPanel()
    {
        // Display correct answers
        foreach (var item in cBallExpectedResult)
        {
            foreach (var prefab in cCorrectAnsPrefab)
            {
                if (prefab.name == item.ToString())
                {
                    Instantiate(prefab, containerCorrectAnswer.transform);
                }
            }
        }
        for (int i = 0; i < cBallYourResult.Count; i++)
        {
            string userAnswer = cBallYourResult[i].ToString();
            string correctAnswer = cBallExpectedResult[i].ToString();

            if (userAnswer == correctAnswer)
            {
                // Correct answer
                for (int j = 0; j < cCorrectAnsPrefab.Length; j++)
                {
                    if (cCorrectAnsPrefab[j].name == userAnswer)
                    {
                        Instantiate(cYourAnsPrefabTrue[j], containerYouranswer.transform);
                    }
                }
            }
            else
            {
                // Incorrect answer
                for (int j = 0; j < cCorrectAnsPrefab.Length; j++)
                {
                    if (cCorrectAnsPrefab[j].name == userAnswer)
                    {
                        Instantiate(cYourAnsPrefabFalse[j], containerYouranswer.transform);
                    }
                }
            }
        }

        resultPanel.SetActive(true);
        CorrectAns.gameObject.SetActive(false);
    }

    public void ResetResult()
    {
        // Clear result containers
        foreach (Transform child in containerCorrectAnswer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in containerYouranswer.transform)
        {
            Destroy(child.gameObject);
        }

        // Clear result data
        cBallYourResult.Clear();
        cBallExpectedResult.Clear();

        // Hide result panel
        resultPanel.SetActive(false);

        // Reset text display
        CorrectAns.gameObject.SetActive(false);
        DecisionText.text = "";
        CorrectAns.text = "";

        // Disable all ball shapes
        foreach (GameObject go in cSpriteForBall)
        {
            go.SetActive(false);
        }

        // Re-enable interaction buttons
        BallButtonInteractable();
    }


}
