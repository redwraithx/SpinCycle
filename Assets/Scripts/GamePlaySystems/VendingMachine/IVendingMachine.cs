


public interface IVendingMachine
{
    void DisplayItemsForSale();
    void SpawnSoldItem(string itemName);
    bool CanBuyItem(int itemForSalesValue, int usersCurrentCashAmount);
    void EnableDisableDisplayUI(bool value = false);

}
