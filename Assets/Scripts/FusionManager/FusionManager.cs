using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using VitaliyNULL.Core;
using VitaliyNULL.MapGeneration;

namespace VitaliyNULL.FusionManager
{
    public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        #region Private Fields

        [SerializeField] private NetworkPrefabRef _carPrefab;
        private NetworkRunner _runner;
        private readonly string _sceneName = "GameScene";
        private const string MapGeneratorPath = "MapGenerator";
        private const string GameManagerPath = "GameManager";
        private bool _leftMove;
        private bool _rightMove;
        private bool _backwardMove;
        [Networked] private bool IsMapGenerated { get; set; }

        private Vector2 _startTouchPos;
        private Vector2 _swipeDelta;
        private float _minDeltaSwipe = 60f;
        private bool _isSwiping;

        //this needs for check what swipe is larger horizontal or vertical
        private float _horizontalSwipe;
        private float _verticalSwipe;

        #endregion

        #region Public Fields

        public Dictionary<PlayerRef, NetworkObject> players = new Dictionary<PlayerRef, NetworkObject>();
        [HideInInspector] public bool _isBrakeDown;

        #endregion

        #region Public Methods

        public void AutoPlay()
        {
            StartGame(GameMode.AutoHostOrClient);
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            _leftMove = Input.GetKey(KeyCode.A);
            _rightMove = Input.GetKey(KeyCode.D);
            _backwardMove = Input.GetKey(KeyCode.S);
        }


        private void ResetSwipe()
        {
            _isSwiping = false;
            _startTouchPos = Vector2.zero;
            _swipeDelta = Vector2.zero;
        }

        #endregion

        #region Private Fields

        private async void StartGame(GameMode mode)
        {
            // Create the Fusion runner and let it know that we will be providing user input
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom",
                PlayerCount = 2,
                Scene = SceneUtility.GetBuildIndexByScenePath($"Scenes/{_sceneName}"),
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }

        #endregion
        

        #region INetworkRunnerCallbacks

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                if (!IsMapGenerated)
                {
                    Debug.Log("Map is Generated");
                    IsMapGenerated = true;
                    runner.Spawn(Resources.Load<GameManager>(GameManagerPath), null, null, player);
                    runner.Spawn(Resources.Load<MapGenerator>(MapGeneratorPath), null, null, player);
                }
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            NetworkInputData data = new NetworkInputData();
            
#if UNITY_EDITOR
            
            if (_leftMove)
            {
                data.ToMoveX = NetworkInputData.MoveLeft;
            }

            if (_rightMove)
            {
                data.ToMoveX = NetworkInputData.MoveRight;
            }

            if (_backwardMove)
            {
                data.ToMoveZ = NetworkInputData.MoveBackward;
            }
            
#endif
            
#if UNITY_ANDROID
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _isSwiping = true;
                _startTouchPos = Input.mousePosition;
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                ResetSwipe();
            }

            _swipeDelta = Vector2.zero;
            if (_isSwiping)
            {
                _swipeDelta = Input.mousePosition;
            }

            _horizontalSwipe = Mathf.Abs(_swipeDelta.x - _startTouchPos.x);
            _verticalSwipe = Mathf.Abs(_swipeDelta.y - _startTouchPos.y);
            if (_horizontalSwipe > _minDeltaSwipe && _horizontalSwipe > _verticalSwipe)
            {
                if (_swipeDelta.x < _startTouchPos.x)
                {
                    //Go Left
                    data.ToMoveX = NetworkInputData.MoveLeft;

                }
                else if (_swipeDelta.x > _startTouchPos.x)
                {
                    //Go right
                    data.ToMoveX = NetworkInputData.MoveRight;
                }

                ResetSwipe();
            }
#endif

            if (_isBrakeDown)
            {
                data.ToMoveZ = NetworkInputData.MoveBackward;
            }

            _leftMove = false;
            _backwardMove = false;
            _rightMove = false;
            input.Set(data);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            SceneManager.LoadScene(0);
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            runner.Shutdown();
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

        #endregion
    }
}