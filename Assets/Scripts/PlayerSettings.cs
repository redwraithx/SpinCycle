using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public GameObject cameraForSense;
    // Start is called before the first frame update
    void Start()
    {
        LoadPlayerSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPlayerSettings()
    {
        if (PlayerPrefs.HasKey("PlayerSensitivity"))
        {
            cameraForSense.GetComponent<CinemachineMouseLook>().mouseSensitivity = PlayerPrefs.GetFloat("PlayerSensitivity");
        }
    }
}
