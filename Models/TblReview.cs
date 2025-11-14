using System;
using System.Collections.Generic;

namespace Hoarding_managment.Models
{
    public partial class TblReview
    {
        public int Id { get; set; }
        public int FkInventoryId { get; set; }
        public int FkCustomerId { get; set; }
        public sbyte Rating { get; set; }
        public string? Comment { get; set; }
        public ulong? IsDelete { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual TblCustomer FkCustomer { get; set; } = null!;
        public virtual TblInventory FkInventory { get; set; } = null!;
    }
}
