using Server.Models.Models;
using System.Collections.Generic;

namespace Server.DL.Memory
{
    public static class ReposMemory
    {
            public static List<Computer> Computers { get; set; } = new List<Computer>();
            public static void Init()
            {
                //Computer
                Computers.Add(new Computer
                {
                    ComputerId = 723456,
                    ComputerBrand = "ALIENWARE"
                });
            }
    }
}
