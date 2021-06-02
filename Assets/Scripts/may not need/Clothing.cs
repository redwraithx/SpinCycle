using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothing : MonoBehaviour, IItem 
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _price;
    [SerializeField] private float _timeAdjustment;
    
    
    // THIS IS OBSOLETE

    public Clothing(int id, string name, string description, int price, float _time)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        TimeAdjustment = _time;
    }

    public Clothing()
    {

    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }
    
    public string Name
    {
        get => _name;
        private set => _name = value;
    }

    public string Description
    {
        get => _description;
        private set => _description = value;
    }

    public int Price
    {
        get => _price;
        set => _price = value;
    }

    public float TimeAdjustment
    {
        get =>  _timeAdjustment;
        private set => _timeAdjustment = value;
    }
}
    


