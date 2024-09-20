

namespace Hoarding_managment.Interface
{
    public interface ICustomer
    {
        //public Task<IEnumerable<TblCustomer>> GetallCustomerAsync(int pageNumber, int pageSize);

        public Task<IEnumerable<TblCustomer>> GetallCustomerAsync( int pageNumber,int pageSize);
        public Task<TblCustomer> UpdateCustomer(TblCustomer model);
        public Task<TblCustomer> DeleteCustomer(int customerId);
        public Task<TblCustomer> AddNewCustomerasAsync(TblCustomer model);
        
        public Task<int> GetCustomerCountAsync();
        public Task<TblCustomer?> GetCustomerById(int id);
        public Task<IEnumerable<TblCustomer>> GetCustomerinfo();

        public  Task<int> CustomerCountAsync();
        public  Task<List<TblCustomer>> SearchCustomersByNameAsync(string name);

        //public List<TblCustomer> GetTblCustomers();
        //public TblCustomer UpdateCustomer(TblCustomer updatedCustomer);
        //public TblCustomer DeleteCustomer(int customerId);
        //public TblCustomer AddNewCustomer(TblCustomer tblCustomer);



    }
}
