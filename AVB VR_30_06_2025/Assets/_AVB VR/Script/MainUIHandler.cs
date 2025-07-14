using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIHandler : MonoBehaviour
{
    [Header("SceneIndex")]
    public int sceneIndex;

    public void LoadDrill_Scene()
    {
        //sceneIndex = DataPassOnUI.instance.sceneIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadHome_Scene()
    {
        SceneManager.LoadScene(0);
    }

    public void AssignVelocity(int velocity)
    {
        DataPassOnUI.instance.velocitySetting = velocity;
    }

    public void AssignAccuracy(int accuracy)
    {
        DataPassOnUI.instance.accuracySetting = accuracy;
    }

    public void AssignPitcher(int pitcher)
    {
        DataPassOnUI.instance.pitchersSetting = pitcher;
    }

    public void AssignSceneIndex(int index)
    {
        sceneIndex = index;
        DataPassOnUI.instance.sceneIndex = index;
    }
}
