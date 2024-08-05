using System.Threading.Tasks;
//using Unity.Services.Economy.Model;
// namespace JKTechnologies.SeensioGo.Economy
// {
    public interface IEconomyManager
    {
        public static IEconomyManager Instance;
        public Task RefreshCurrencyBalancesAsync();
        public long GetCurrencyBalanceByIdAsync(string currencyId);

        public Task RefreshInventoryAsync();
        public int GetNumberItemsByIdAsync(string itemKey);
        public Task<bool> MakeVirtualPurchaseByIdAsync(string virtualPurchaseId);
    }
//}