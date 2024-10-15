

namespace Hoarding_managment.Repository
{
    public class AuthRepository : IAuth
    {
        private readonly db_hoarding_managementContext _context;

        public AuthRepository(db_hoarding_managementContext db_hoarding_managementContext)
        {
            _context = db_hoarding_managementContext;
        }

        public async Task<TblUser?> LoginAsync(string username, string password)
        {
            // Use case-insensitive comparison for username
            var user = await _context.TblUsers
                .FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower() && x.Password.ToLower() == password.ToLower() && x.IsDelete==0);

            return user;
        }


        public async Task<TblUser?> GetUserByIdAsync(int id)
        {
            return await _context.TblUsers
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == 0);
        }
    }

}
