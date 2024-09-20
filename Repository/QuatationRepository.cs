using AngleSharp.Dom;
using Hoarding_managment.Models;
using HoardingManagement.Interface;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using static iTextSharp.text.pdf.AcroFields;

namespace HoardingManagement.Repository
{
    public class QuotationRepository : IQuotation
    {
        private readonly db_hoarding_managementContext _context;

        public QuotationRepository(db_hoarding_managementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TblQuotation>> GetAllQuotationsAsync(int pageNumber, int pageSize)
        {
            return await _context.TblQuotations
                .Where(x => x.IsDelete == 0)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<QuatationViewModel>> GetAllQuotationsListAsync(int pageNumber, int pageSize)
        {
            List<QuatationViewModel> lstquotation = new List<QuatationViewModel>();

            var quotations = await _context.TblQuotations
                .Where(x => x.IsDelete == 0)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            foreach (var item in quotations)
            {


                var vendorid = _context.TblQuotationitems.Where(x=>x.FkQuotationId==item.Id).Select(x=>x.FkVendorId).FirstOrDefault();
                var name = _context.TblVendors.FirstOrDefault(x => x.Id == vendorid)?.VendorName;
                QuatationViewModel model = new QuatationViewModel
                {
                    Id = item.Id,
                    QuotationNumber = item.QuotationNumber,
                    CreatedAt = item.CreatedAt,
                    CreatedBy = item.CreatedBy,
                    UpdatedBy = item.UpdatedBy,
                    FkCustomerId = item.FkCustomerId,
                    VendorName = name,
                    CustomerName = await _context.TblCustomers
                        .Where(x => x.Id == item.FkCustomerId && x.IsDelete == 0)
                        .Select(x => x.CustomerName)
                        .FirstOrDefaultAsync(),
                    Totalhoarding = _context.TblQuotationitems.Where(x => x.FkQuotationId == item.Id).Count(),
                };
                lstquotation.Add(model);
            }
            return lstquotation;
        }


        public async Task<IEnumerable<QuatationViewModel>> GetCustomerDetailsAsync()
        {
            return (IEnumerable<QuatationViewModel>)_context.TblQuotations.Where(x => x.IsDelete == 0).ToList();
        }

        public async Task<TblQuotation> GetQuotationByIdAsync(int id) => await _context.TblQuotations
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == 0);

        public async Task<TblQuotation> GetQuotationByNumberAsync(string quotationNumber)
        {
            return await _context.TblQuotations
                .Where(x => x.IsDelete == 0 && x.QuotationNumber == quotationNumber)
                .OrderByDescending(x => x.QuotationNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetQuotationCountAsync()
        {
            return await _context.TblQuotations.CountAsync();
        }

        public async Task<int> AddQuotationAsync(TblQuotation selectedQuotation)
        {
            await _context.TblQuotations.AddAsync(selectedQuotation);
            await _context.SaveChangesAsync();

            return selectedQuotation.Id;
        }

        public async Task<List<TblQuotation>> AddQuotationsAsync(List<TblQuotation> models)
        {
            await _context.TblQuotations.AddRangeAsync(models);
            await _context.SaveChangesAsync();
            return models;
        }

        public async Task<TblQuotation> UpdateQuotationAsync(TblQuotation model)
        {
            _context.TblQuotations.Update(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<int> DeleteQuotationAsync(int id)
        {
            var quotation = await _context.TblQuotations
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == 0);

            if (quotation != null)
            {
                quotation.IsDelete = 1;
                _context.TblQuotations.Update(quotation);
                await _context.SaveChangesAsync();
                return 1;
            }

            return 0;
        }

        public async Task<List<InventoryitemViewmodel>> GetInventoryItemsAsync()
        {
            return await _context.TblInventoryitems
                .Where(x => x.IsDelete == 0)
                .Select(item => new InventoryitemViewmodel
                {
                    Id = item.Id,
                    Image = item.Image,
                    City = item.City,
                    Area = item.Area,
                    Width = item.Width,
                    Height = item.Height,
                    Rate = item.Rate,
                    BookingStatus = item.BookingStatus,
                    IsLight = item.IsLight,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                    FkInventoryId = item.FkInventoryId
                })
                .ToListAsync();
        }

      
        public async  Task<QuatationDetaileViewModel> GetQuotationByIdDetailAsync(int id)
        {
            // Retrieve the quotation details
            var quotation = await _context.TblQuotations
                .Where(q => q.Id == id && q.IsDelete == 0) // Using `false` for `bit(1)`
                .Select(q => new
                {
                    q.Id,
                    q.FkCustomerId,
                    q.QuotationNumber,
                    q.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (quotation == null)
            {
                return null; // Or handle the case when the quotation is not found
            }

            // Retrieve the quotation items
            var quotationItems = await _context.TblQuotationitems
                .Where(x => x.FkQuotationId == id && x.IsDelete == 0)
                .Select(x => new
                {
                    x.City,
                    x.Area,
                    x.Location,
                    x.IsLight,
                    x.Width,
                    x.Height,
                    x.BookingStatus,
                    x.Rate,
                    x.VendorAmt,
                    x.MarginAmt,
                    x.Image,
                    x.LocationDescription,
                    x.FkQuotationId
                })
                .ToListAsync();

            // Retrieve customer details
            var customer = await _context.TblCustomers
                .Where(c => c.Id == quotation.FkCustomerId)
                .Select(c => new { c.CustomerName, c.City, c.BusinessName })
                .FirstOrDefaultAsync();

            var vendorid = _context.TblQuotationitems.Where(x => x.FkQuotationId == x.FkQuotationId).Select(x => x.FkVendorId ).FirstOrDefault();
            var name = _context.TblVendors.FirstOrDefault(x => x.Id == vendorid)?.VendorName;
            // Retrieve vendor details

            var inveryid = _context.TblQuotationitems.Where(x => x.FkQuotationId == x.FkQuotationId).Select(x => x.FkInventory).FirstOrDefault();

            // Retrieve inventory details
            var inventory = await _context.TblInventories
                .Where(i => i.Id == inveryid)
                .Select(i => new { i.Location, i.Width, i.Height })
                .FirstOrDefaultAsync();

            // Map to ViewModel
            var viewModel = new     QuatationDetaileViewModel

        {
            Items = quotationItems.Select(item => new QuotationItemViewModel
                {
                    City = item.City,
                    Area = item.Area,
                    Location = item.Location,
                    Width = item.Width,
                    Height = item.Height,
                    Rate = item.Rate,
                    VendorAmt = item.VendorAmt,
                    MarginAmt = item.MarginAmt,
                    Image = item.Image,
                    LocationDescription = item.LocationDescription
                }).ToList(),
                
                Id=quotation.Id,
                 CreatedAt=quotation.CreatedAt,
                 QuotationNumber=quotation.QuotationNumber,
                CustomerName = customer?.CustomerName,
                VendorName =name,
                Width = inventory?.Width,
                Height = inventory?.Height
            };

            return viewModel;
        }

    }
}
