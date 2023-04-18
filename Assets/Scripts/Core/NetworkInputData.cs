using Fusion;

namespace VitaliyNULL.Core
{
    public struct NetworkInputData : INetworkInput
    {
        public const byte MOVE_LEFT = 0x01;
        public const byte MOVE_RIGHT = 0x02;
        public byte ToMoveX;

    }
}