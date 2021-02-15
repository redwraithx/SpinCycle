
using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _price;
    [SerializeField] private float _timeAjustment;


    private void Awake()
    {
        gameObject.tag = "Item";

        gameObject.layer = LayerMask.NameToLayer("Items");
    }


    public Item(string name, string description, int price, float _time)
    {
        Name = name;
        Description = description;
        Price = price;
        TimeAjustment = _time;
    }

    public Item()
    {

    }

    public string Name
    {
        get =>  _name;
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

    public float TimeAjustment
    {
        get => _timeAjustment;
        private set => _timeAjustment = value;
    }
}



