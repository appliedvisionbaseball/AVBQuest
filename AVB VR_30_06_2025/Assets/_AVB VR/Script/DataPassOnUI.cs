using TMPro;
using UnityEngine;

public class DataPassOnUI : MonoBehaviour
{
    public static DataPassOnUI instance;

    [Header("Update based on UI seletion")]
    public int pitchersSetting;  // Steve, Bud, Dante, Chris, Brian, Hiro, Steve2
    public int velocitySetting; // 50-55, 55-60, 60-65, 65-70, 75-80, 80-85, 85-90, 90-95, 95-100, 98-103
    public int accuracySetting; // PinPoint = 95%, Accurate = 90%, Finding the zone = 80%, Wild = 50%
    public int drillsetting;    // Visual Acuity, Pitch Type, Swing Trigger, Post Up, Free Round
    public int sceneIndex;

    [Header("Pitcher Detail")]
    public pitcherDetails[] pitcherDetails;

    private void Awake()
    {
        // Ensure only one instance of DataPassOnUI exists
        if (instance != null)
        {
            Destroy(gameObject); // Destroy the duplicate instance
            return;
        }

        instance = this; // Set the current instance
        DontDestroyOnLoad(gameObject); // Make this object persistent across scene loads
    }
  
}


[System.Serializable]

public class pitcherDetails
{
    public string pitcherName;
    public string pitcherHeight;
    public string pitcherThrow;
    public string pitcherArmslot;
    public string pitcherStyle;
    public Sprite pitcherSprite;
}
