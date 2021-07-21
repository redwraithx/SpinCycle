using UnityEngine;
using UnityEngine.Events;


namespace GamePlaySystems.Utilities
{
    [System.Serializable]
    public class InteractableObjectEvent : UnityEvent<GameObject>
    {
    }
    
    
    public class ItemTypeForUsingItem : MonoBehaviour
    {
        [Header("Item Types valid to use with this object")]
        public ItemType[] itemType;

        [Space, Header("Only Add ONE event function per machine.")]
        public InteractableObjectEvent thisObjectEvent;
        
        

        private void Start()
        {
            if(thisObjectEvent.GetPersistentEventCount() <= 0)
                Debug.LogError("Error Machine script is missing event function or its invalid");
        }


        public ItemTypeForItem UseItem(GameObject other, ItemType[] itemTypeToMatch)
        {
            // if other is null
            if (!other || itemTypeToMatch.Length <= 0)
                return null;

            ItemTypeForItem[] othersItemType = other.GetComponentsInChildren<ItemTypeForItem>();
            

            if (othersItemType.Length <= 0)
                return null;

            foreach (var itemOther in othersItemType)
            {
                foreach (var item in itemTypeToMatch)
                {
                    //Debug.Log($"itemOther: {itemOther.itemType}, item: {item}");
                    
                    if (itemOther.itemType == item)
                    {
                        return itemOther;
                    }
                }
            }

            // returns false if no match is found
            return null;
        }


        public ItemTypeForItem GetItemsGameObject(GameObject othersGO)
        {
            return othersGO.GetComponentInChildren<ItemTypeForItem>();
        }


    }

}