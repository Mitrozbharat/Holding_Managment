
using Hoarding_managment.Models;

namespace Hoarding_managment.Repository
{
    public class VendorRepository:IVendor
    {
        private readonly db_hoarding_managementContext _context;

        public VendorRepository(db_hoarding_managementContext db_hoarding_managementContext)
        {
            _context = db_hoarding_managementContext;
        }
        public async Task<IEnumerable<TblVendor>> GetAllVendorsAsync(int pageNumber, int pageSize)
        {
            return await _context.TblVendors
        .Where(v => v.IsDelete == 0)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
        }
        public async Task<TblVendor> GetVendorByIdAsync(int id)
        {
            return await _context.TblVendors.FindAsync(id);
        }
        public async Task<TblVendor> AddVendorAsync(TblVendor vendor)
        {
            // Add the vendor entity to the context
            _context.TblVendors.Add(vendor);
            // Save the changes asynchronously to the database
            await _context.SaveChangesAsync();

            return vendor;
        }
        public async Task<TblVendor> UpdateVendorAsync(int id, TblVendor vendor)
        {
            _context.TblVendors.Update(vendor);
            await _context.SaveChangesAsync();

            return vendor;
        }
        public TblVendor DeleteVendor(int id)
        {
            var vendor =  _context.TblVendors.FirstOrDefault(x=>x.Id==id);
            if (vendor != null)
            {
                  vendor.IsDelete = 1; // Set the Isdelete column to 1
                _context.TblVendors.Update(vendor); // Update the entity
                 _context.SaveChanges(); // Save changes to the database
            }
            return vendor;
        }
        public async Task<int> GetVendorCountAsync()
        {
            return await _context.TblVendors.CountAsync(v => v.IsDelete == 0);

        }

     

    }

}