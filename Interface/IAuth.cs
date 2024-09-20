


namespace Hoarding_managment.Interface
{
    public interface IAuth
    {
        public TblUser Login(string username,String password);
        public TblUser GetUserById(int id);
    }
}
