using Fusion;

namespace VitaliyNULL.Core
{
    public struct NetworkInputData : INetworkInput
    {
        #region Public Fields

        public const byte MoveLeft = 0x01;
        public const byte MoveRight = 0x02;
        public const byte MoveForward = 0x03;
        public byte ToMoveX;
        public byte ToMoveZ;

        #endregion
    }
}