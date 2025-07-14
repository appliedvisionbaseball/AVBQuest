using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [Header("Screen Button List")]
    public List<Button> pitchersButtons; 
    public List<Button> velocityButtons; 
    public List<Button> AccuracyButtons; 
    public List<Button> TrainingDrillButtons;
    public List<Button> BatStyleButtons;
    public List<Button> TimeOfDayButtons;
    public List<Button> LocationButtons;
    public List<Button> CalibrationButtons;
    public List<Button> ChooseBatButtons;
    public List<Button> COptionButtons;

    [Header("Screen Next Button")]
    public Button screen1NextButton;
    public Button screen2NextButton;


    [Header("Button Effect")]
    // Scaling parameters
    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);
    public float buttonAnimationDuration = 0.5f;
    public float buttonAnimationPauseBeforeShrink = 0.1f;

    bool isVelocitySelected, isPitcherselected, isAccuracySelected = false;
    bool isDrillSelected, isTimeOfDaySelected, isLocationselected = false;

    public void ButtonScaleAnim(Transform buttonObj)
    {
        StartCoroutine(ScaleOverTime(buttonObj, targetScale, buttonAnimationDuration));
    }

    private IEnumerator ScaleOverTime(Transform obj, Vector3 toScale, float duration)
    {
        Vector3 originalScale = obj.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            obj.localScale = Vector3.Slerp(originalScale, toScale, elapsedTime / duration);
            yield return null;
        }

        // Ensure final scale is exactly the target scale
        obj.localScale = toScale;

        // Optional: Animate back to the original scale
        yield return new WaitForSeconds(buttonAnimationPauseBeforeShrink); // Pause before shrinking
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            obj.localScale = Vector3.Slerp(toScale, originalScale, elapsedTime / duration);
            yield return null;
        }

        obj.localScale = originalScale;
    }

    public void OnPitchersHighlightButton(Button selectedPitcherButton)
    {
        foreach (Button button in pitchersButtons)
        {
            if (button == selectedPitcherButton)
            {
                Debug.Log("Selected Pitchers Button: " + button.name);
                StartCoroutine(ScaleOverTime(button.transform, targetScale, buttonAnimationDuration));
                button.transform.GetChild(1).gameObject.SetActive(true);
                isPitcherselected = true;

                if (isVelocitySelected && isPitcherselected && isAccuracySelected)
                {
                    screen1NextButton.interactable = true;
                    screen1NextButton.GetComponent<Image>().raycastTarget = true;
                }
            }
            else
            {
                button.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void OnCSelectionHighlightButton(Button selectedCShapeButton)
    {
        foreach (Button button in COptionButtons)
        {
            if (button == selectedCShapeButton)
            {
                Debug.Log("Selected C Button: " + button.name);
                StartCoroutine(ScaleOverTime(button.transform, targetScale, buttonAnimationDuration));
                button.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                button.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void OnVelocityHighlightButton(Button selectedVelocityButton)
    {
        foreach (Button button in velocityButtons)
        {
            if (button == selectedVelocityButton)
            {
                Debug.Log("Selected Velocity Button: " + button.name);
                StartCoroutine(ScaleOverTime(button.transform, targetScale, buttonAnimationDuration));
                button.transform.GetChild(1).gameObject.SetActive(true);
                isVelocitySelected = true;

                if (isVelocitySelected && isPitcherselected && isAccuracySelected)
                {
                    screen1NextButton.interactable = true;
                    screen1NextButton.GetComponent<Image>().raycastTarget = true;
                }
            }
            else
            {
                button.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void OnAccuracyHighlightButton(Button selectedAccuracyButton)
    {
        foreach (Button button in AccuracyButtons)
        {
            if (button == selectedAccuracyButton)
            {
                Debug.Log("Selected Accuracy Button: " + button.name);
                StartCoroutine(ScaleOverTime(button.transform, targetScale, buttonAnimationDuration));
                button.transform.GetChild(1).gameObject.SetActive(true);
                isAccuracySelected = true;

                if (isVelocitySelected && isPitcherselected && isAccuracySelected)
                {
                    screen1NextButton.interactable = true;
                    screen1NextButton.GetComponent<Image>().raycastTarget = true;
                }
            }
            else
            {
                button.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void OnTrainingDrillsHighlightButton(Button selectedTrainingDrillsButton)
    {
        foreach (Button button in TrainingDrillButtons)
        {
            if (button == selectedTrainingDrillsButton)
            {
                Debug.Log("Selected TrainingDrills Button: " + button.name);
                StartCoroutine(ScaleOverTime(button.transform, targetScale, buttonAnimationDuration));
                button.transform.GetChild(1).gameObject.SetActive(true);
                isDrillSelected = true;

                if (isDrillSelected && isTimeOfDaySelected && isLocationselected)
                {
                    screen2NextButton.interactable = true;
                    screen2NextButton.GetComponent<Image>().raycastTarget = true;
                }
            }
            else
            {
                button.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void OnBatStyleHighlightButton(Button selectedBatStyleButton)
    {
        foreach (Button button in BatStyleButtons)
        {
            if (button == selectedBatStyleButton)
            {
                Debug.Log("Selected BatStyle Button: " + button.name);
                StartCoroutine(ScaleOverTime(button.transform, targetScale, buttonAnimationDuration));
                button.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                button.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void OnTimeOfDayHighlightButton(Button selectedTimeOfDayButton)
    {
        foreach (Button button in TimeOfDayButtons)
        {
            if (button == selectedTimeOfDayButton)
            {
                Debug.Log("Selected TimeOfDay Button: " + button.name);
                StartCoroutine(ScaleOverTime(button.transform, targetScale, buttonAnimationDuration));
                button.transform.GetChild(1).gameObject.SetActive(true);
                isTimeOfDaySelected = true;

                if (isDrillSelected && isTimeOfDaySelected && isLocationselected)
                {
                    screen2NextButton.interactable = true;
                    screen2NextButton.GetComponent<Image>().raycastTarget = true;
                }
            }
            else
            {
                button.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void OnLocationHighlightButton(Button selectedLocationButton)
    {
        foreach (Button button in LocationButtons)
        {
            if (button == selectedLocationButton)
            {
                Debug.Log("Selected Location Button: " + button.name);
                StartCoroutine(ScaleOverTime(button.transform, targetScale, buttonAnimationDuration));
                button.transform.GetChild(1).gameObject.SetActive(true);
                isLocationselected = true;

                if (isDrillSelected && isTimeOfDaySelected && isLocationselected)
                {
                    screen2NextButton.interactable = true;
                    screen2NextButton.GetComponent<Image>().raycastTarget = true;
                }
            }
            else
            {
                button.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void OnCalibrationHighlightButton(Button selectedCalibrationButton)
    {
        foreach (Button button in CalibrationButtons)
        {
            if (button == selectedCalibrationButton)
            {
                Debug.Log("Selected Calibration Button: " + button.name);
                StartCoroutine(ScaleOverTime(button.transform, targetScale, buttonAnimationDuration));
                button.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                button.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void OnChooseBatHighlightButton(Button selectedChooseBatButton)
    {
        foreach (Button button in ChooseBatButtons)
        {
            if (button == selectedChooseBatButton)
            {
                Debug.Log("Selected ChooseBat Button: " + button.name);
                StartCoroutine(ScaleOverTime(button.transform, targetScale, buttonAnimationDuration));
                button.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                button.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}
