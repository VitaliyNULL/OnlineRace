using Fusion;
using UnityEngine;

namespace VitaliyNULL.Props
{
    public class PropCarMove : NetworkBehaviour
    {
        #region Private Fields

        // private NetworkRigidbody _rigidbody;
        private Vector3 _directionToMove;
        private NetworkTransform _networkTransform;

        #endregion

        #region Public Fields

        private float _speed = 20;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            // _rigidbody ??= GetComponent<NetworkRigidbody>();
            _networkTransform ??= GetComponent<NetworkTransform>();
        }

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            // _rigidbody ??= GetComponent<NetworkRigidbody>();
            _networkTransform ??= GetComponent<NetworkTransform>();

            // if (_rigidbody.Rigidbody.rotation.y > 0 || _rigidbody.Rigidbody.rotation.y < 0)
            // {
            //     _directionToMove = -Vector3.forward;
            // }
            // else
            // {
            //     _directionToMove = Vector3.forward;
            // }
            if (_networkTransform.transform.rotation.y > 0 || _networkTransform.transform.rotation.y < 0)
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
            // if (_rigidbody.Rigidbody.position.z > 6000 || _rigidbody.Rigidbody.position.z < 0)
            // {
            //     Runner.Despawn(Object);
            //     return;
            // }
            //
            // _rigidbody.Rigidbody.MovePosition(
            //     _rigidbody.Rigidbody.position + _directionToMove * _speed * Runner.DeltaTime);
            if (_networkTransform.transform.position.z > 6000 || _networkTransform.transform.position.z < 0)
            {
                Runner.Despawn(Object);
                return;
            }

            _networkTransform.TeleportToPosition(_networkTransform.transform.position +
                                                 _directionToMove * _speed * Runner.DeltaTime);
        }

        #endregion
    }
}