public interface IVendingMachine
{
    void DisplayItemsForSale();

    void SpawnSoldItem(int itemPrefabID);

    bool CanBuyItem(int itemForSalesValue, int usersCurrentCashAmount);

    void EnableDisableDisplayUI(bool value = false);
}