using Server.Models.Models;
using System.Collections.Concurrent;

namespace Server.DL.Memory
{
    public static class InMemoryDb
    {
        public static ConcurrentQueue<Computer> Computers { get; set; } = new ConcurrentQueue<Computer>();

    }
}
