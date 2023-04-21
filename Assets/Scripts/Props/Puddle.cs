namespace VitaliyNULL.Props
{
    public class Puddle : Prop
    {
        public override void Interact(Player.Player player)
        {
            if (!player.IsInvulnerable)
            {
                player.playerMove.StopPickingUpSpeed(6f);
                player.playerMove.ForwardSpeed -= 10f;
            }
        }
    }
}