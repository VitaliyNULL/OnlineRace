using Fusion;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace VitaliyNULL.Test
{
    public class TestTick : NetworkBehaviour
    {
        [Networked] private TickTimer _timer { get; set; }
        [Networked][SerializeField] private bool CanGo { get; set; }


        public override void FixedUpdateNetwork()
        {
            if (CanGo)
            {
                GoRight();
            }
        }

        private void GoRight()
        {
            if (_timer.ExpiredOrNotRunning(Runner))
            {
                _timer = TickTimer.CreateFromSeconds(Runner, Runner.DeltaTime);
                transform.position = Vector3.Lerp(transform.position,
                    transform.position + Vector3.right * 2 * Runner.DeltaTime, 1);
            }
        }
    }
}