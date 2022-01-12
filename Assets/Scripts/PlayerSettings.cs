using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public GameObject cameraForSense;

    private void Start()
    {
        LoadPlayerSettings();
    }

    public void LoadPlayerSettings()
    {
        if (PlayerPrefs.HasKey("PlayerSensitivity"))
        {
            cameraForSense.GetComponent<CinemachineMouseLook>().mouseSensitivity = PlayerPrefs.GetFloat("PlayerSensitivity");
        }
    }
}