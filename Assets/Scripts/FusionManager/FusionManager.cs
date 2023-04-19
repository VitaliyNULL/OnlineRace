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
        private bool _leftMove;
        private bool _rightMove;
        [Networked] private bool IsMapGenerated { get; set; }

        #endregion

        #region Public Fields

        public Dictionary<PlayerRef, NetworkObject> players = new Dictionary<PlayerRef, NetworkObject>();

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
            _leftMove |= Input.GetKey(KeyCode.A);
            _rightMove |= Input.GetKey(KeyCode.D);
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
                Scene = SceneUtility.GetBuildIndexByScenePath($"Scenes/{_sceneName}"),
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }

        #endregion

        #region RPC

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_UpdatePlayers(PlayerRef playerRef, NetworkObject networkObject)
        {
            players[playerRef] = networkObject;
        }

        #endregion

        #region INetworkRunnerCallbacks

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                if (!IsMapGenerated)
                {
                    IsMapGenerated = true;
                    runner.Spawn(Resources.Load<MapGenerator>(MapGeneratorPath));
                }

                Vector3 spawnPosition =
                    new Vector3(3 + (player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 5, -9, 0);

                NetworkObject playerObject =
                    runner.Spawn(_carPrefab, spawnPosition, Quaternion.identity, player);
                RPC_UpdatePlayers(player, playerObject);
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            NetworkInputData data = new NetworkInputData();
            if (_leftMove)
            {
                data.ToMoveX |= NetworkInputData.MoveLeft;
            }

            if (_rightMove)
            {
                data.ToMoveX |= NetworkInputData.MoveRight;
            }

            _leftMove = false;
            _rightMove = false;
            input.Set(data);
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

        #endregion
    }
}