using Server.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.BL.Interface
{
    public interface IComputerService
    {
        Task<Computer> Create(Computer computer);
        Task<Computer> Update(Computer computer);
        Task Delete(int ComputerId);
        Task<IEnumerable<Computer>> GetAll();
    }
}
