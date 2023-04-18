using Fusion;
using UnityEngine;

namespace VitaliyNULL.Props
{
    public class PropCarMove : NetworkBehaviour
    {
        private NetworkRigidbody _rigidbody;
        public float speed;
        private Vector3 _directionToMove;

        public override void Spawned()
        {
            _rigidbody = GetComponent<NetworkRigidbody>();
            if (_rigidbody.Rigidbody.rotation.y > 0 || _rigidbody.Rigidbody.rotation.y < 0)
            {
                _directionToMove = -Vector3.forward;
            }
            else
            {
                _directionToMove = Vector3.forward;
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (_rigidbody.Rigidbody.position.z > 6000 || _rigidbody.Rigidbody.position.z < 0)
            {
                Runner.Despawn(Object);
            }
            _rigidbody.Rigidbody.MovePosition(
                _rigidbody.Rigidbody.position + _directionToMove * speed * Runner.DeltaTime);
        }
    }
}