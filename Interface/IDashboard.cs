using Microsoft.EntityFrameworkCore;

namespace Hoarding_managment.Interface
{
    public interface IDashboard
    {
        public Task<List<InventoryViewModel>> GetAllHoarldingInvenrotyAsync(int pageNumber, int pageSize);
        public Task<int> GetAllHoarldingInvenrotyCountAsync();

        public Task<TblInventory> GetInvetroyByIdAsync(int id);
        public Task<int> DeleteInventryAsync(int id);
        public Task<TblInventoryitem> CreateInventryItemsAsync(TblInventoryitem model);

        public Task<TblInventory> AddNewInventoryAsync(TblInventory model);
        public  Task<TblInventory> UpdateInventoryItemAsync(TblInventory model);
        public Task<TblInventory> GetInventryItemsByIdAsync(int id);
        public Task<int> InventryItemscountAsync();
        public  Task<int> InventoryCountAsync();
        public Task<int> DeleteInventryItemsAsync(int id);
        public Task<List<InventoryitemViewmodel>> GetInventryItemsAsync();
        public Task<List<InventoryitemViewmodel>> get();
        public Task<int> AddQuatationsAsync(QuotationItemListViewModel selectedItems);
        public  Task<QuotationItemListViewModel> addCampaign(QuotationItemListViewModel selectedItems);
        public  Task<TblInventory> UploadExcelAsync(TblInventory inventory);


        public Task<InventoryViewModel> SearchByInventoryNameAsync(string name);
    }
}
