namespace VitaliyNULL.Props
{
    public class CrashedCar : Prop
    {
        public override void Interact(Player.Player player)
        {

            player.playerMove.ForwardSpeed -= 20f;
            Runner?.Despawn(Object);
        }
    }
}