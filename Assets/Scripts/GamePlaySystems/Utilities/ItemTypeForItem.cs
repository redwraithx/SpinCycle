using UnityEngine;
using EnumSpace;

namespace GamePlaySystems.Utilities
{

    public class ItemTypeForItem : MonoBehaviour
    {

        public ItemType itemType;
        public LaundryType laundryType;

        public enum ItemType
        {
            None,
            ClothingDirty,
            ClothingWet,
            ClothingUnfolded,
            ClothingDone,
            SabotageWaterGun,
            SabotageClothing
        }
    }

    
}