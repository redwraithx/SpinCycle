using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TShirt : MonoBehaviour, IItem 
{
    [SerializeField] private string _name = "";
    [SerializeField] private string _description = "";
    [SerializeField] private int _price = 0;
    [SerializeField] private float _timeAdjustment = 0f;


    // THIS IS OBSOLETE

    public TShirt(string name, string description, int price, float timeAdjustment)
    {
        Name = name;
        Description = description;
        Price = price;
        TimeAjustment = timeAdjustment;
    }

    public TShirt()
    {

    }

    public string Name
    {
        get
        {
            return _name;
        }
        private set
        {
            _name = value;
        }
    }

    public string Description
    {
        get
        {
            return _description;
        }
        private set
        {
            _description = value;
        }
    }

    public int Price
    {
        get
        {
            return _price;
        }
        private set
        {
            _price = value;
        }
    }

    public float TimeAjustment
    {
        get => _timeAdjustment;
        private set
        {
            _timeAdjustment = value;
        }
    }
}
    


