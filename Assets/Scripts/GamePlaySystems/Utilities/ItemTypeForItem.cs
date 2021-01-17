using UnityEngine;

namespace GamePlaySystems.Utilities
{

    public class ItemTypeForItem : MonoBehaviour
    {

        public ItemType itemType;

        public enum ItemType
        {
            None,
            ClothingClean,
            ClothingDirty,
            SabotageWaterGun,
            SabotageClothing
        }
    }

    
}