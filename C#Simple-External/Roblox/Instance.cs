using System.Collections.Generic;
using SimpleExternal.Core.Memory;
using SimpleExternal.Core.Update;

namespace SimpleExternal.Roblox
{
    public class Instance
    {
        public ulong Address { get; private set; }
        public bool Valid => Address != 0;

        public Instance(ulong address)
        {
            Address = address;
        }

        public string GetName()
        {
            ulong namePtr = Mem.Read<ulong>(Address + Offsets.Name);
            if (namePtr == 0)
                return string.Empty;

            return Mem.ReadString(namePtr);
        }

        public List<Instance> GetChildren()
        {
            var children = new List<Instance>();

            ulong start = Mem.Read<ulong>(Address + Offsets.Children);
            ulong end = Mem.Read<ulong>(start + Offsets.ChildrenEnd);

            for (ulong ptr = Mem.Read<ulong>(start); ptr != end; ptr += 0x10)
            {
                ulong childAddr = Mem.Read<ulong>(ptr);
                children.Add(new Instance(childAddr));
            }

            return children;
        }

        public Instance FindFirstChild(string name)
        {
            foreach (var child in GetChildren())
            {
                if (child.GetName() == name)
                    return child;
            }

            return new Instance(0);
        }
    }
}