using System;
using System.Collections.Generic;

namespace Hoarding_managment.Models
{
    public partial class TblPayment
    {
        public int Id { get; set; }
        public int? FkCustomerId { get; set; }
        public int? FkCampaignId { get; set; }
        public int? FkQuotationId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public string? TransactionRef { get; set; }
        public ulong? IsDelete { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual TblCampaign? FkCampaign { get; set; }
        public virtual TblCustomer? FkCustomer { get; set; }
        public virtual TblQuotation? FkQuotation { get; set; }
    }
}
