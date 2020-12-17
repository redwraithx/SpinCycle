using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PointCounter : MonoBehaviour
{
    public Text points;
    public float managerPoints;
    void Start()
    {
       managerPoints = GameManager.Instance.points;
    }

    // Update is called once per frame
    void Update()
    {
        points.text = managerPoints.ToString();
    }
}
