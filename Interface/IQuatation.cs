using Hoarding_managment.Models;
using System.Runtime.CompilerServices;

namespace HoardingManagement.Interface
{
    public interface IQuotation
    { public Task<List<TblQuotation>> AddQuotationsAsync(List<TblQuotation> models); 
        public Task<IEnumerable<TblQuotation>> GetAllQuotationsAsync(int pageNumber, int pageSize);
        public Task<List<QuatationViewModel>> GetAllQuotationsListAsync(int pageNumber, int pageSize); // Added alternative name
        public Task<TblQuotation> GetQuotationByIdAsync(int id); // Renamed for clarity
        public Task<TblQuotation> GetQuotationByNumberAsync(string quotationNumber); // Added quotation number parameter
        public Task<int> GetQuotationCountAsync();
        public Task<int> AddQuotationAsync(TblQuotation selectedQuotation); // Renamed for clarity
        public Task<TblQuotation> UpdateQuotationAsync(TblQuotation model); // Renamed for clarity
        public Task<int> DeleteQuotationAsync(int id); // Renamed for clarity
        public Task<QuatationDetaileViewModel> GetQuotationByIdDetailAsync(int id);
     

    }
}
