using Server.BL.Interface;
using Server.DL.Interface;
using Server.DL.Repository;
using Server.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.BL.Service
{
    public class ComputerService : IComputerService
    {
        private readonly IComputerRepository _computerRepository;

        public ComputerService(ComputerRepository computerRepository)
        {
            _computerRepository = computerRepository;
        }
        public async Task<Computer> Create(Computer computer)
        {
            return await _computerRepository.Create(computer);
        }

        public async Task Delete(int ComputerId)
        {
            await _computerRepository.Delete(ComputerId);
        }

        public async Task<IEnumerable<Computer>> GetAll()
        {
            return await _computerRepository.GetAll();
        }

        public async Task<Computer> Update(Computer computer)
        {
            return await _computerRepository.Update(computer);
        }
    }
}
