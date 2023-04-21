namespace VitaliyNULL.Props
{
    public class ManHole: Prop
    {
        public override void Interact(Player.Player player)
        {

            player.playerMove.ForwardSpeed -= 10f;
            Runner?.Despawn(Object);
        }
    }
}