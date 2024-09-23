namespace Hoarding_managment.Data
{
    public class InventoryPagedViewModel
    {
        public List<InventoryViewModel>? InventoryViewModel { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } // Add this property
        public string? SearchQuery { get; set; }
    }
}
