using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleExternal.Core.Memory
{
    public static class Mem
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(uint dwAccess, bool bInherit, uint dwPid);

        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProc, IntPtr lpBase, byte[] lpBuffer, int nSize, out int lpRead);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProc, IntPtr lpBase, byte[] lpBuffer, int nSize, out int lpWritten);

        const uint PROCESS_ALL_ACCESS = 0x1F0FFF;

        public static IntPtr Handle;
        public static uint ProcessId;

        public static bool Attach(string process)
        {
            var results = Process.GetProcessesByName(process.Replace(".exe", ""));
            if (results.Length == 0)
                return false;

            ProcessId = (uint)results[0].Id;
            Handle = OpenProcess(PROCESS_ALL_ACCESS, false, ProcessId);

            return Handle != IntPtr.Zero;
        }

        public static T Read<T>(ulong address) where T : struct
        {
            var buffer = new byte[Marshal.SizeOf<T>()];
            ReadProcessMemory(Handle, (IntPtr)address, buffer, buffer.Length, out _);

            var pinned = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var result = Marshal.PtrToStructure<T>(pinned.AddrOfPinnedObject());
            pinned.Free();

            return result;
        }

        public static void Write<T>(ulong address, T value) where T : struct
        {
            var buffer = new byte[Marshal.SizeOf<T>()];

            var pinned = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            Marshal.StructureToPtr(value, pinned.AddrOfPinnedObject(), false);
            pinned.Free();

            WriteProcessMemory(Handle, (IntPtr)address, buffer, buffer.Length, out _);
        }

        public static string ReadString(ulong address, int maxLength = 256)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < maxLength; i++)
            {
                var buf = new byte[1];
                ReadProcessMemory(Handle, (IntPtr)(address + (ulong)i), buf, 1, out _);

                if (buf[0] == 0x0)
                    break;

                sb.Append((char)buf[0]);
            }

            return sb.ToString();
        }

        public static ulong GetModuleBase(string moduleName)
        {
            var results = Process.GetProcessesByName(moduleName.Replace(".exe", ""));
            if (results.Length == 0)
                return 0;

            foreach (ProcessModule mod in results[0].Modules)
            {
                if (mod.ModuleName.Equals(moduleName, StringComparison.OrdinalIgnoreCase))
                    return (ulong)mod.BaseAddress;
            }

            return 0;
        }
    }
}