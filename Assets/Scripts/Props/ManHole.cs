namespace VitaliyNULL.Props
{
    public class ManHole : Prop
    {
        public override void Interact(Player.Player player)
        {
            if (!player.IsInvulnerable)
            {
                player.playerMove.ForwardSpeed -= 10f;
                player.playerMove.TeleportToBack();
                player.IsInvulnerable = true;
            }
        }
    }
}