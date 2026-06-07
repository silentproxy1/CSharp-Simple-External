using System;
using SimpleExternal.Core.Memory;
using SimpleExternal.Roblox;

namespace SimpleExternal
{
    internal class Program
    {
        static int Main()
        {
            Console.Title = "SimpleExternal";

            if (!Mem.Attach("RobloxPlayerBeta.exe"))
            {
                Console.WriteLine("roblox not open bro");
                Console.ReadKey();
                return 1;
            }

            Console.WriteLine("Attached to roblox");

            Rbx.SetWalkSpeed(100f);
            Console.WriteLine("ws set to 100");

            Rbx.SetJumpPower(200f);
            Console.WriteLine("jump set to 200");

            Console.WriteLine("\nhit any key to close");
            Console.ReadKey();
            return 0;
        }
    }
}