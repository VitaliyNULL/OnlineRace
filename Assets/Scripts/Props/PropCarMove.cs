using Fusion;
using UnityEngine;

namespace VitaliyNULL.Props
{
    public class PropCarMove : NetworkBehaviour
    {
        #region Private Fields

        private NetworkRigidbody _rigidbody;
        private Vector3 _directionToMove;

        #endregion

        #region Public Fields

        public float speed;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _rigidbody ??= GetComponent<NetworkRigidbody>();
        }

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            _rigidbody ??= GetComponent<NetworkRigidbody>();
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
                return;
            }

            _rigidbody.Rigidbody.MovePosition(
                _rigidbody.Rigidbody.position + _directionToMove * speed * Runner.DeltaTime);
        }

        #endregion
    }
}