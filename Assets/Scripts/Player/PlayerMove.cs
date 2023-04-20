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
        private bool _isMoving = false;
        private float _sideMoveSpeed = 10f;
        private float _forwardSpeed = 40f;
        private float _multiplayer = 1f;
        private NetworkRigidbody _rigidbody;
        private Vector3 _toMove;

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
            _currentPositionIndex = 0;
            _toMove = _rigidbody.Rigidbody.position;
        }

        public override void FixedUpdateNetwork()
        {
            _rigidbody.Rigidbody.MovePosition(new Vector3(_rigidbody.Rigidbody.position.x,
                _rigidbody.Rigidbody.position.y,
                _rigidbody.Rigidbody.position.z + _forwardSpeed * Runner.DeltaTime));
            _forwardSpeed += 0.01f;
            if (GetInput(out NetworkInputData data))
            {
                if ((data.ToMoveZ & NetworkInputData.MoveBackward) != 0)
                {
                    

                    Debug.Log(_forwardSpeed);
                }
                else
                {
                    if (_forwardSpeed > 40f)
                    {
                        _forwardSpeed -= 0.01f;
                    }
                    else
                    {
                        _forwardSpeed = 40f;
                    }

                    Debug.Log(_forwardSpeed);
                }

                if ((data.ToMoveX & NetworkInputData.MoveLeft) != 0 && !_isMoving)
                {
                    if (_currentPositionIndex != 0)
                    {
                        Debug.Log("Move Left");
                        _isMoving = true;
                        _currentPositionIndex--;
                        StartCoroutine(StartMovingLeft(_toMoveXPositions[_currentPositionIndex]));
                    }
                }
                else if ((data.ToMoveX & NetworkInputData.MoveRight) != 0 && !_isMoving)
                {
                    if (_currentPositionIndex != _toMoveXPositions.Length - 1)
                    {
                        Debug.Log("Move Right");
                        _isMoving = true;
                        _currentPositionIndex++;
                        StartCoroutine(StartMovingRight(_toMoveXPositions[_currentPositionIndex]));
                    }
                }
            }

            if (_multiplayer < 3)
            {
                _multiplayer += 0.001f;
            }

            _toMove += Vector3.forward * _forwardSpeed * _multiplayer * Runner.DeltaTime;
        }

        #endregion

        #region Coroutines

        private IEnumerator StartMovingLeft(float toMoveX)
        {
            float xPositionToMove = _rigidbody.Rigidbody.position.x;
            while (_rigidbody.Rigidbody.position.x > toMoveX)
            {
                xPositionToMove -= Mathf.Abs(_sideMoveSpeed * Runner.DeltaTime);
                _rigidbody.Rigidbody.MovePosition(new Vector3(xPositionToMove, _rigidbody.Rigidbody.position.y,
                    _rigidbody.Rigidbody.position.z));

                yield return new WaitForEndOfFrame();
            }

            _rigidbody.Rigidbody.position =
                new Vector3(toMoveX, _rigidbody.Rigidbody.position.y, _rigidbody.Rigidbody.position.z);
            _isMoving = false;
        }

        private IEnumerator StartMovingRight(float toMoveX)
        {
            float xPositionToMove = _rigidbody.Rigidbody.position.x;
            while (_rigidbody.Rigidbody.position.x < toMoveX)
            {
                xPositionToMove += Mathf.Abs(_sideMoveSpeed * Runner.DeltaTime);
                _rigidbody.Rigidbody.MovePosition(new Vector3(xPositionToMove, _rigidbody.Rigidbody.position.y,
                    _rigidbody.Rigidbody.position.z));

                yield return new WaitForEndOfFrame();
            }

            _rigidbody.Rigidbody.position =
                new Vector3(toMoveX, _rigidbody.Rigidbody.position.y, _rigidbody.Rigidbody.position.z);
            _isMoving = false;
        }

        #endregion
    }
}