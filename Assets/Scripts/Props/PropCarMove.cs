using Fusion;
using UnityEngine;
using VitaliyNULL.FusionManager;

namespace VitaliyNULL.Props
{
    public class PropCarMove : NetworkBehaviour
    {
        #region Private Fields

        // private NetworkRigidbody _rigidbody;
        private Vector3 _directionToMove;
        private GameManager _gameManager;
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
            _networkTransform ??= GetComponent<NetworkTransform>();
            _gameManager ??= FindObjectOfType<GameManager>();
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
            if (!_gameManager.IsGameStarted) return;
            if (_networkTransform.transform.position.z > 6000 || _networkTransform.transform.position.z < 0)
            {
                Runner.Despawn(Object);
                return;
            }

            _networkTransform.transform.position = Vector3.Lerp(_networkTransform.transform.position,
                _networkTransform.transform.position + _directionToMove * _speed * Runner.DeltaTime, 1);
        }

        #endregion
    }
}