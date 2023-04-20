namespace VitaliyNULL.Props
{
    public class Puddle : Prop
    {
        public override void Interact(Player.Player player)
        {
            player.playerMove.StopPickingUpSpeed(6f);
            player.playerMove.ForwardSpeed -= 10f;
        }
    }
}