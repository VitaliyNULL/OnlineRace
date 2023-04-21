namespace VitaliyNULL.Props
{
    public class Nitro : Prop
    {
        public override void Interact(Player.Player player)
        {
            player.playerMove.ForwardSpeed += 20;
            player.playerMove.MultiplayerForwardSpeed += 0.3f;
            Runner?.Despawn(Object);
        }
    }
}