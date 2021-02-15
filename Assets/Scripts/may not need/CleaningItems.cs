using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningItems : MonoBehaviour, IItem
{
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private int price;
    [SerializeField] private float timeChange;
    [SerializeField] private ItemQuality quality;
    [SerializeField] private float _timeAjustment;

    public enum ItemQuality
    {
        poor,
        low,
        medium,
        high
    }


    public CleaningItems(string name, string description, int price, float _time)
    {
        Name = name;
        Description = description;
        Price = price;
        TimeAjustment = _time;
    }

    public CleaningItems()
    {

    }

    public string Name
    {
        get
        {
            return name;
        }
        private set
        {
            name = value;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }
        private set
        {
            description = value;
        }
    }

    public int Price
    {
        get
        {
            return price;
        }
        private set
        {
            price = value;
        }
    }

    public float TimeChange
    {
        get
        {
            return timeChange;
        }
        private set
        {
            timeChange = value;
        }
    }
    public ItemQuality Quality
    {
        get
        {
            return quality;
        }
        private set
        {
          quality = value;
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
