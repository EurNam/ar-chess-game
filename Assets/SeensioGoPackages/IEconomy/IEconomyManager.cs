// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Unity.Services.Economy.Model;

// namespace JKTechnologies.SeensioGo.Economy
// {
//     public interface IEconomyManager
//     {
//         public static IEconomyManager Instance;
//         #region CURRENCIES
//         public Task RefreshCurrencyBalancesAsync();
//         public long GetCurrencyBalanceByIdAsync(CurrencyEnum currencyEnum);
//         public Task<long> DecreaseCurrency(CurrencyEnum currencyEnum, int amount);
//         public Task<long> IncreaseCurrency(CurrencyEnum currencyEnum, int amount);
//         #endregion

//         #region INVENTORY
//         public Task RefreshInventoryAsync();
//         public int GetNumberItemsByIdAsync(string itemKey);
//         public PlayersInventoryItem[] GetInventoryItems();
//         public Task<bool> UpdateInventoryItem(string InventoryItemId,  Dictionary<string, object> instanceData);
//         #endregion

//         #region VIRTUAL PURCHASES
//         public Task<MakeVirtualPurchaseResult> MakeVirtualPurchaseByIdAsync(string virtualPurchaseId);
//         #endregion
//     }

//     public enum CurrencyEnum
//     {
//         Gold = 1,
//     }
// }