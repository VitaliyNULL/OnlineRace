using System.Collections;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.Player
{
    public class PlayerMove : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private Player _player;
        private readonly float[] _toMoveXPositions = { -8, -3, 3, 8 };
        private int _currentPositionIndex;
        private bool _isMoving;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        private float _sideMoveSpeed = 10f;
#elif UNITY_ANDROID

        private float _sideMoveSpeed = 20f;

#endif
        private readonly float _forwardSpeedMin = 40f;
        private readonly float _forwardSpeedMax = 120f;
        private float _forwardSpeed { get; set; }
        private float _multiplayerForwardSpeed { get; set; }
        private float _distance;

        private NetworkTransform _networkTransform;
        private bool _isPickingUpSpeed = true;

        #endregion

        #region Private Properties

        public float Distance
        {
            get => _distance;
            set
            {
                _distance = value;
                if (HasInputAuthority)
                    _player.gameManager.gameUI.UpdatePlayerDistance(_distance);
            }
        }

        #endregion

        #region Public Properties

        public float MultiplayerForwardSpeed
        {
            get => _multiplayerForwardSpeed;
            set { _multiplayerForwardSpeed = Mathf.Clamp(value, 1, 2f); }
        }

        public float ForwardSpeed
        {
            get => _forwardSpeed;
            set { _forwardSpeed = Mathf.Clamp(value, _forwardSpeedMin, _forwardSpeedMax); }
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _networkTransform ??= GetComponent<NetworkTransform>();
        }

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            _networkTransform ??= GetComponent<NetworkTransform>();
            _currentPositionIndex = 0;
            _forwardSpeed = 40;
            _multiplayerForwardSpeed = 1;
        }


        public override void FixedUpdateNetwork()
        {
            if (!_player.gameManager.IsGameStarted || _player.finished) return;
            _networkTransform.Transform.position = Vector3.Lerp(_networkTransform.Transform.position,
                _networkTransform.Transform.position + Vector3.forward * ForwardSpeed * Runner.DeltaTime, 1);

            // if (HasInputAuthority)
            // {
            Distance = _player.gameManager.finishTile.transform.position.z - transform.position.z;
            if (Distance < 0)
            {
                if (!HasStateAuthority)
                {
                    RPC_Finish();
                    _player.OpenFinishUI();
                }
                else
                {
                    _player.finished = true;
                    _player.OpenFinishUI();
                }
            }
            // }

            if (GetInput(out NetworkInputData data))
            {
                if (_isPickingUpSpeed)
                {
                    ForwardSpeed += 0.01f * MultiplayerForwardSpeed;
                }

                MultiplayerForwardSpeed -= 0.001f;
                if ((data.ToMoveZ & NetworkInputData.MoveBackward) != 0)
                {
                    ForwardSpeed -= 1f;
                }

                if ((data.ToMoveX & NetworkInputData.MoveLeft) != 0 && !_isMoving)
                {
                    if (_currentPositionIndex != 0)
                    {
                        _isMoving = true;
                        _currentPositionIndex--;
                        StartCoroutine(StartMovingLeft(_toMoveXPositions[_currentPositionIndex]));
                    }
                }
                else if ((data.ToMoveX & NetworkInputData.MoveRight) != 0 && !_isMoving)
                {
                    if (_currentPositionIndex != _toMoveXPositions.Length - 1)
                    {
                        _isMoving = true;
                        _currentPositionIndex++;
                        StartCoroutine(StartMovingRight(_toMoveXPositions[_currentPositionIndex]));
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        public void StopPickingUpSpeed(float time)
        {
            StartCoroutine(StopAddingSpeed(time));
        }

        public void TeleportToBack()
        {
            _networkTransform.TeleportToPosition(_networkTransform.transform.position + Vector3.back * 5);
        }

        #endregion

        #region Coroutines

        private IEnumerator StartMovingLeft(float toMoveX)
        {
            float xPositionToMove = _networkTransform.transform.position.x;
            while (_networkTransform.transform.position.x > toMoveX)
            {
                xPositionToMove -= Mathf.Abs(_sideMoveSpeed * Runner.DeltaTime);
                _networkTransform.transform.position = Vector3.Lerp(_networkTransform.transform.position,
                    new Vector3(xPositionToMove,
                        _networkTransform.transform.position.y,
                        _networkTransform.transform.position.z), 1);
                yield return new WaitForEndOfFrame();
            }

            _networkTransform.transform.position = Vector3.Lerp(_networkTransform.transform.position,
                new Vector3(toMoveX, _networkTransform.transform.position.y,
                    _networkTransform.transform.position.z), 1);

            _isMoving = false;
        }

        private IEnumerator StopAddingSpeed(float time)
        {
            _isPickingUpSpeed = false;
            yield return new WaitForSeconds(time);
            _isPickingUpSpeed = true;
        }

        private IEnumerator StartMovingRight(float toMoveX)
        {
            float xPositionToMove = _networkTransform.transform.position.x;
            while (_networkTransform.transform.position.x < toMoveX)
            {
                xPositionToMove += Mathf.Abs(_sideMoveSpeed * Runner.DeltaTime);
                _networkTransform.transform.position = Vector3.Lerp(_networkTransform.transform.position,
                    new Vector3(xPositionToMove,
                        _networkTransform.transform.position.y,
                        _networkTransform.transform.position.z), 1);
                yield return new WaitForEndOfFrame();
            }

            _networkTransform.transform.position = Vector3.Lerp(_networkTransform.transform.position,
                new Vector3(toMoveX, _networkTransform.transform.position.y,
                    _networkTransform.transform.position.z), 1);

            _isMoving = false;
        }

        #endregion

        #region RPC

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, InvokeLocal = false)]
        private void RPC_Finish()
        {
            _player.finished = true;
        }

        #endregion
    }
}