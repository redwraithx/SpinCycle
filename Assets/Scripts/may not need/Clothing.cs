using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothing : MonoBehaviour, IItem 
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _price;
    [SerializeField] private float _timeAjustment;
    
    
    // THIS IS OBSOLETE

    public Clothing(string name, string description, int price, float _time)
    {
        Name = name;
        Description = description;
        Price = price;
        TimeAjustment = _time;
    }

    public Clothing()
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
        get
        {
            return _timeAjustment;
        }
        private set
        {
            _timeAjustment = value;
        }
    }
}
    


