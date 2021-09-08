using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Rendering;

public class TutorialPlayerPoints : MonoBehaviour
{
    private int points = 0;
    public int Points
    {
        get => points;
        set => points = value;
    }


}
