namespace VitaliyNULL.Props
{
    public class Nitro : Prop
    {
        public override void Interact(Player.Player player)
        {
            player.playerMove.ForwardSpeed += 5;
            player.playerMove.MultiplayerForwardSpeed += 0.1f;
            Runner?.Despawn(Object);
        }
    }
}