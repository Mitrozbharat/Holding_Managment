using Hoarding_managment.Interface;
using Hoarding_managment.Models;
using System.Runtime.InteropServices;

namespace Hoarding_managment.Repository
{
    public class CustomerRepository : ICustomer
    {
        private readonly db_hoarding_managementContext _context;

        public CustomerRepository(db_hoarding_managementContext db_hoarding_managementContext)
        {
            _context = db_hoarding_managementContext;
        }
        public async Task<IEnumerable<TblCustomer>> GetallCustomerAsync(int pageNumber, int pageSize)
        {

            return await _context.TblCustomers
                .Where(v => v.IsDelete == 0)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        }

        public async Task<TblCustomer> AddNewCustomerasAsync(TblCustomer model)
        {
            _context.TblCustomers.Add(model);
            await _context.SaveChangesAsync();
            return model;

        }
        public async Task<TblInventory> AddNewInventoryAsync(TblInventory model)
        {
            _context.TblInventories.Add(model);
            await _context.SaveChangesAsync();
            return model;

        }
        public async Task<TblCustomer> UpdateCustomer(TblCustomer model)
        {
            _context.TblCustomers.Update(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<TblCustomer> DeleteCustomer(int id)
        {
            var existcustomer = _context.TblCustomers.FirstOrDefault(x => x.Id == id);
            if (existcustomer != null)
            {

                existcustomer.IsDelete = 1;
                await _context.SaveChangesAsync();
                return existcustomer;
            }
            return existcustomer;
        }
        public async Task<int> GetCustomerCountAsync()
        {
            return await _context.TblCustomers.CountAsync(v => v.IsDelete == 0);
        }

        public async Task<IEnumerable<TblCustomer>> GetCustomerinfo()
        {
            return await _context.TblCustomers
                .Where(c => c.IsDelete == 0)
                .ToListAsync();
        }

        public async Task<TblCustomer> GetCustomerById(int id)
        {
            var customer = _context.TblCustomers.FirstOrDefault(v => v.Id == id && v.IsDelete == 0);

            if (customer != null)
            {
                return customer;
            }
            return customer;
        }

        public async Task<int> CustomerCountAsync()
        {
            return await _context.TblCustomers
                                 .Where(x => x.IsDelete == 0)
            .CountAsync();
     
        }

        public async Task<List<TblCustomer>> SearchCustomersByNameAsync(string name)
        {
            var lowerCaseName = name.ToLower();
            return await _context.TblCustomers
                                 .Where(c => c.IsDelete == 0 && c.CustomerName.ToLower().Contains(lowerCaseName))
                                 .ToListAsync();
        }




    }
}
