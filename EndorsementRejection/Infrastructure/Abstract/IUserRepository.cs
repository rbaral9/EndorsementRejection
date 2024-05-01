using EndorsementRejection.Models.Entities;

namespace EndorsementRejection.Infrastructure.Abstract
{
    public interface IUserRepository
    {
        public List<EndoUser> EndoUserList();
        public List<ApprovalUser> ApprovalUserList();
    }
}