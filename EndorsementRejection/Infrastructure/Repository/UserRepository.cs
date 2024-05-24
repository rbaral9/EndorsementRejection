using EndorsementRejection.Data;
using EndorsementRejection.Infrastructure.Abstract;
using EndorsementRejection.Models.Entities;

namespace EndorsementRejection.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository() { }

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }





        public List<EndoUser> EndoUserList()
        {
            var list = new List<EndoUser>();
            list = _context.EndoUsers.ToList();
            //list.Add(new EndoUser { Id = 0, UserName = "Select", Password = "", Role = "" });
            list = list.OrderBy(user => user.UserName).ToList();
            return list;
        }
        public List<ApprovalUser> ApprovalUserList()
        {
            var list = new List<ApprovalUser>();
            list = _context.ApprovalUsers.ToList();
           // list.Add(new ApprovalUser { Id = 0, UserName = "Select", Password = "", Role = "" });
            list = list.OrderBy(user => user.UserName).ToList();
            return list;
        }


    }
}
