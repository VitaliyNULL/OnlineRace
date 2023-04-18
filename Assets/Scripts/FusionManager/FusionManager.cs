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
        [SerializeField] private NetworkPrefabRef carPrefab;
        private NetworkRunner _runner;
        private string _sceneName = "GameScene";
        private const string MAPGENERATOR_PATH = "MapGenerator";
        private bool _leftMove = false;
        private bool _rightMove = false;

        public void AutoPlay()
        {
            StartGame(GameMode.AutoHostOrClient);
        }

        private void Update()
        {
            _leftMove |= Input.GetKey(KeyCode.A);
            _rightMove |= Input.GetKey(KeyCode.D);
        }

        async void StartGame(GameMode mode)
        {
            // Create the Fusion runner and let it know that we will be providing user input
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            // Start or join (depends on gamemode) a session with a specific name
            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom",
                Scene = SceneUtility.GetBuildIndexByScenePath($"Scenes/{_sceneName}"),
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            runner.Spawn(Resources.Load<MapGenerator>(MAPGENERATOR_PATH));
            runner.Spawn(carPrefab, new Vector3(-8, 4, 0), Quaternion.identity, player);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            NetworkInputData data = new NetworkInputData();
            if (_leftMove)
            {
                data.ToMoveX |= NetworkInputData.MOVE_LEFT;
            }

            if (_rightMove)
            {
                data.ToMoveX |= NetworkInputData.MOVE_RIGHT;
            }

            _leftMove = false;
            _rightMove = false;
            input.Set(data);
            Debug.Log("Input");
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
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
    }
}