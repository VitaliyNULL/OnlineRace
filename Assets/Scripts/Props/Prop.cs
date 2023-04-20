using Fusion;

namespace VitaliyNULL.Props
{
    public abstract class Prop : NetworkBehaviour
    {
        public abstract void Interact(Player.Player player);
    }
}