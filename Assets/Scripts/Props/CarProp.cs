namespace VitaliyNULL.Props
{
    public class CarProp : Prop
    {
        public override void Interact(Player.Player player)
        {
            if (!player.IsInvulnerable)
            {
                player.playerMove.ForwardSpeed -= 20f;
                player.playerMove.TeleportToBack();
                player.IsInvulnerable = true;
            }
        }
    }
}