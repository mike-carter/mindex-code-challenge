using challenge.Models;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation GetById(string id);
        Compensation Add(Compensation compensation);
        Task SaveAsync();
    }
}
