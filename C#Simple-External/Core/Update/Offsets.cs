namespace SimpleExternal.Core.Update
{
    public static class Offsets
    {
        // DataModel
        public const ulong FakeDataModel = 0x78ff228;
        public const ulong FakeDmToDm = 0x1d0;

        // Instance
        public const ulong Name = 0xb0;
        public const ulong Children = 0x78;
        public const ulong ChildrenEnd = 0x8;

        // Players
        public const ulong LocalPlayer = 0x138;

        // Humanoid
        public const ulong WalkSpeed = 0x1dc;
        public const ulong WalkSpeedCheck = 0x3c4;
        public const ulong JumpPower = 0x1b0;
    }
}