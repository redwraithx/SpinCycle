using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TShirt : MonoBehaviour, IItem 
{
    [SerializeField] private int _id = 0;
    [SerializeField] private string _name = "";
    [SerializeField] private string _description = "";
    [SerializeField] private int _price = 0;
    [SerializeField] private float _timeAdjustment = 0f;




    public TShirt(int id, string name, string description, int price, float timeAdjustment)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        TimeAdjustment = timeAdjustment;
    }

    public TShirt()
    {

    }

    public int Id
    {
        get => _id;
        private set => _id = value;
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
        private set => _price = value;
    }

    public float TimeAdjustment
    {
        get => _timeAdjustment;
        private set => _timeAdjustment = value;
    }
}
    


