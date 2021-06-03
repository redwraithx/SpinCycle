using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningItems : MonoBehaviour, IItem
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private int price;
    [SerializeField] private float timeChange;
    [SerializeField] private ItemQuality quality;
    [SerializeField] private float _timeAdjustment;

    public enum ItemQuality
    {
        poor,
        low,
        medium,
        high
    }


    public CleaningItems(int id, string name, string description, int price, float _time)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        TimeAdjustment = _time;
    }

    public CleaningItems()
    {

    }

    public int Id
    {
        get => id;
        private set => id = value;
    }

    public string Name
    {
        get => name;
        private set => name = value;
    }

    public string Description
    {
        get =>  description;
        private set => description = value;
    }

    public int Price
    {
        get => price;
        set => price = value;
    }

    public float TimeChange
    {
        get => timeChange;
        private set => timeChange = value;
    }
    public ItemQuality Quality
    {
        get =>  quality;
        private set => quality = value;
    }

    public float TimeAdjustment
    {
        get =>  _timeAdjustment;
        private set => _timeAdjustment = value;
    }

}
