
using UnityEngine;

[System.Serializable]
public class VendingIndex : MonoBehaviour
{
    public string Name;
    public string Price;
    public string Description;

    public VendingIndex(string name, string desc, string price)
        {
        Name = name;
        Price = price;
        Description = desc;
        Debug.Log($"Name: {name}, Desc: {desc}, Price: {price}");
    }


}
