using SimpleExternal.Core.Memory;
using SimpleExternal.Core.Update;

namespace SimpleExternal.Roblox
{
    public static class Rbx
    {
        public static Instance GetDataModel()
        {
            ulong baseAddr = Mem.GetModuleBase("RobloxPlayerBeta.exe");
            ulong fakeDm = Mem.Read<ulong>(baseAddr + Offsets.FakeDataModel);
            ulong realDm = Mem.Read<ulong>(fakeDm + Offsets.FakeDmToDm);

            return new Instance(realDm);
        }

        public static Instance GetService(string name)
        {
            return GetDataModel().FindFirstChild(name);
        }

        public static Instance GetLocalHumanoid()
        {
            var workspace = GetService("Workspace");
            var players = GetService("Players");

            ulong lpAddr = Mem.Read<ulong>(players.Address + Offsets.LocalPlayer);
            var localPlayer = new Instance(lpAddr);

            var character = localPlayer.FindFirstChild("Character");
            if (!character.Valid)
                character = workspace.FindFirstChild(localPlayer.GetName());

            return character.FindFirstChild("Humanoid");
        }

        public static void SetWalkSpeed(float speed)
        {
            var humanoid = GetLocalHumanoid();
            if (!humanoid.Valid) return;

            Mem.Write(humanoid.Address + Offsets.WalkSpeed, speed);
            Mem.Write(humanoid.Address + Offsets.WalkSpeedCheck, speed);
        }

        public static void SetJumpPower(float power)
        {
            var humanoid = GetLocalHumanoid();
            if (!humanoid.Valid) return;

            Mem.Write(humanoid.Address + Offsets.JumpPower, power);
        }
    }
}