
using UnityEngine;

public class ExitSceneFadeIn : MonoBehaviour
{
    public GameObject blackOutObject = null;
    
    public void DisableFadeInUI()
    {
        if(blackOutObject)
            blackOutObject.SetActive(false);
    }
    
    public void EnableFadeInUI()
    {
        if(blackOutObject)
            blackOutObject.SetActive(true);
    }
}
