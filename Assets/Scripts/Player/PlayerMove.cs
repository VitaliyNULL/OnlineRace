using System.Collections;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.Player
{
    public class PlayerMove : NetworkBehaviour
    {
        #region Private Fields

        private readonly float[] _toMoveXPositions = { -8, -3, 3, 8 };
        private int _currentPositionIndex;
        private bool _isMoving;
        private float _sideMoveSpeed = 10f;
        private readonly float _forwardSpeedMin = 40f;
        private readonly float _forwardSpeedMax = 200f;
        private float _forwardSpeed;

        private float _multiplayerForwardSpeed = 1f;

        // private NetworkRigidbody _rigidbody;
        private NetworkTransform _networkTransform;
        private bool _isPickingUpSpeed = true;

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
            // _rigidbody ??= GetComponent<NetworkRigidbody>();
            _networkTransform ??= GetComponent<NetworkTransform>();
        }

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            // _rigidbody ??= GetComponent<NetworkRigidbody>();
            _networkTransform ??= GetComponent<NetworkTransform>();
            _currentPositionIndex = 0;
        }


        public override void FixedUpdateNetwork()
        {
            // _rigidbody.Rigidbody.MovePosition(new Vector3(_rigidbody.Rigidbody.position.x,
            //     _rigidbody.Rigidbody.position.y,
            //     _rigidbody.Rigidbody.position.z + ForwardSpeed * Runner.DeltaTime));
            _networkTransform.transform.position = Vector3.Lerp(_networkTransform.transform.position,
                _networkTransform.transform.position + Vector3.forward * ForwardSpeed * Runner.DeltaTime, 1);

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

        #endregion

        #region Coroutines

        private IEnumerator StartMovingLeft(float toMoveX)
        {
            // float xPositionToMove = _rigidbody.Rigidbody.position.x;

            // while (_rigidbody.Rigidbody.position.x > toMoveX)
            // {
            //     xPositionToMove -= Mathf.Abs(_sideMoveSpeed * Runner.DeltaTime);
            //     _rigidbody.Rigidbody.MovePosition(new Vector3(xPositionToMove, _rigidbody.Rigidbody.position.y,
            //         _rigidbody.Rigidbody.position.z));
            //
            //     yield return new WaitForEndOfFrame();
            // }
            //
            // _rigidbody.Rigidbody.position =
            //     new Vector3(toMoveX, _rigidbody.Rigidbody.position.y, _rigidbody.Rigidbody.position.z);
            float xPositionToMove = _networkTransform.transform.position.x;
            while (_networkTransform.transform.position.x > toMoveX)
            {
                xPositionToMove -= Mathf.Abs(_sideMoveSpeed * Runner.DeltaTime);
                _networkTransform.transform.position = Vector3.Lerp(_networkTransform.transform.position,
                    new Vector3(xPositionToMove,
                        _networkTransform.transform.position.y,
                        _networkTransform.transform.position.z), 1);
                // _networkTransform.TeleportToPosition(new Vector3(xPositionToMove,
                //     _networkTransform.transform.position.y,
                //     _networkTransform.transform.position.z));

                yield return new WaitForEndOfFrame();
            }

            _networkTransform.transform.position = Vector3.Lerp(_networkTransform.transform.position,
                new Vector3(toMoveX, _networkTransform.transform.position.y,
                    _networkTransform.transform.position.z), 1);
            // _networkTransform.transform.position =
            //     new Vector3(toMoveX, _networkTransform.transform.position.y, _networkTransform.transform.position.z);
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
            // float xPositionToMove = _rigidbody.Rigidbody.position.x;
            // while (_rigidbody.Rigidbody.position.x < toMoveX)
            // {
            //     xPositionToMove += Mathf.Abs(_sideMoveSpeed * Runner.DeltaTime);
            //     _rigidbody.Rigidbody.MovePosition(new Vector3(xPositionToMove, _rigidbody.Rigidbody.position.y,
            //         _rigidbody.Rigidbody.position.z));
            //
            //     yield return new WaitForEndOfFrame();
            // }
            //
            // _rigidbody.Rigidbody.position =
            //     new Vector3(toMoveX, _rigidbody.Rigidbody.position.y, _rigidbody.Rigidbody.position.z);
            float xPositionToMove = _networkTransform.transform.position.x;
            while (_networkTransform.transform.position.x < toMoveX)
            {
                xPositionToMove += Mathf.Abs(_sideMoveSpeed * Runner.DeltaTime);
                _networkTransform.transform.position = Vector3.Lerp(_networkTransform.transform.position,
                    new Vector3(xPositionToMove,
                        _networkTransform.transform.position.y,
                        _networkTransform.transform.position.z), 1);
                // _networkTransform.TeleportToPosition(new Vector3(xPositionToMove,
                //     _networkTransform.transform.position.y,
                //     _networkTransform.transform.position.z));

                yield return new WaitForEndOfFrame();
            }

            _networkTransform.transform.position = Vector3.Lerp(_networkTransform.transform.position,
                new Vector3(toMoveX, _networkTransform.transform.position.y,
                    _networkTransform.transform.position.z), 1);

            // _networkTransform.transform.position =
            //     new Vector3(toMoveX, _networkTransform.transform.position.y, _networkTransform.transform.position.z);
            _isMoving = false;
        }

        #endregion
    }
}