using Hoarding_managment.Models;

namespace Hoarding_managment.Interface
{
    public interface IVendor
    {

       public Task<IEnumerable<TblVendor>> GetAllVendorsAsync(int pageNumber, int pageSize);
        public Task<TblVendor> GetVendorByIdAsync(int id);
        public Task<TblVendor> AddVendorAsync(TblVendor vendor);
        public Task<TblVendor> UpdateVendorAsync(int id,TblVendor vendor);
        public TblVendor DeleteVendor(int id);

        public Task<int> GetVendorCountAsync();
    }
       
    }
