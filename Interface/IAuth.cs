
using Hoarding_managment;

namespace Hoarding_managment.Interface
{
    public interface IAuth
    {
        public Task<TblUser> LoginAsync(string username,String password);
        public Task<TblUser> GetUserByIdAsync(int id);
    }
}
