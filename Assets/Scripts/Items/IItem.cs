using UnityEngine;

public interface IItem
{
    int Id { get; }

    string Name { get; }

    string Description { get; }

    int Price { get; set; }

    float TimeAdjustment { get; }

    Sprite sprite { get; }

    int OwnerID { get; set; }
}