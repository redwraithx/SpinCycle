using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// THIS WILL GO ON A EMPTY GAME OBJECT CALLED "MenuController"
public class MenuController : MonoBehaviour
{
    public void OnClickCharacterPick(int whichCharacter)
    {
        if (PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySelectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);
            
        }
    }
    
    
}
