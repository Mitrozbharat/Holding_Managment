namespace Hoarding_managment.Data
{
    public class InventoryIteamPagedListModel
    {
        public List<InventoryitemViewmodel> Inventoryitems { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
