using OdinWeb.Models.Obj;

namespace OdinWeb.Models.Data.Interfaces
{
    public interface IBranchModel
    {
        public List<Branch> GetBranch();
        public List<Branch> GetBranchesAll();
        public bool PostBranch(Branch branch);

        public Branch GetBranchById(int id);

        public bool PutBranchById(Branch branch);

        public bool DeleteBranchById(int id);
    }
}
