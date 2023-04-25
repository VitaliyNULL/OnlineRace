using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.InGameUI;
using VitaliyNULL.MapGeneration;

namespace VitaliyNULL.FusionManager
{
    public class GameManager : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private NetworkPrefabRef _prefabRef;
        private Dictionary<PlayerRef, Player.Player> _players = new Dictionary<PlayerRef, Player.Player>();
        private MapGenerator _mapGenerator;

        [Networked(OnChanged = nameof(OnTimerChange))]
        private float _time { get; set; }

        private GameTime _gameTime;
        [HideInInspector] public TimerUI timerUI;
        [HideInInspector] public GameUI gameUI;
        private WaitingUI _waitingUI;
        [HideInInspector] public MapTile finishTile;

        [Networked] private bool _isGameStarted { get; set; }

        #endregion

        #region Public Properties

        public bool IsGameStarted
        {
            get => _isGameStarted;
            set => _isGameStarted = value;
        }

        #endregion

        #region Private Methods

        private static void OnTimerChange(Changed<GameManager> changed)
        {
            changed.Behaviour._gameTime.SetTime(Mathf.FloorToInt(changed.Behaviour._time));
            changed.Behaviour.timerUI.ChangeTime(changed.Behaviour._gameTime.ToString());
        }

        #endregion

        #region Public Methods

        public void SetFinish(MapTile mapTile)
        {
            finishTile = mapTile;
        }

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            timerUI = FindObjectOfType<TimerUI>();
            _waitingUI = FindObjectOfType<WaitingUI>();
            gameUI = FindObjectOfType<GameUI>();
            timerUI.gameObject.SetActive(false);
            gameUI.gameObject.SetActive(false);
            Debug.Log(Runner.SessionInfo.PlayerCount + "/" + Runner.SessionInfo.MaxPlayers + "Players now");
            if (Runner.SessionInfo.PlayerCount == Runner.SessionInfo.MaxPlayers)
            {
                RPC_ChangeStartTimer();
                RPC_SpawnAllPlayer();
            }

            _gameTime = new GameTime();
        }

        public override void FixedUpdateNetwork()
        {
            foreach (var player in _players)
            {
                if (_players[Runner.LocalPlayer].playerMove.Distance > player.Value.playerMove.Distance)
                {
                    gameUI.UpdatePlayerPosition(1);
                }
                else
                {
                    gameUI.UpdatePlayerPosition(2);
                }
            }

            if (!HasStateAuthority) return;
            if (IsGameStarted)
            {
                _time += Runner.DeltaTime;
            }
        }

        #endregion


        private IEnumerator WaitToStart()
        {
            timerUI.ChangeStartTimer("1");
            yield return new WaitForSeconds(1f);
            timerUI.ChangeStartTimer("2");
            yield return new WaitForSeconds(1f);
            timerUI.ChangeStartTimer("3");
            yield return new WaitForSeconds(1f);
            timerUI.ChangeStartTimer("GO!");
            IsGameStarted = true;
            yield return new WaitForSeconds(0.5f);
            timerUI.DisableStartTimer();
        }

        #region RPC

        [Rpc]
        private void RPC_ChangeStartTimer()
        {
            StartCoroutine(WaitToStart());
        }

        [Rpc]
        private void RPC_SpawnAllPlayer()
        {
            _waitingUI.gameObject.SetActive(false);
            gameUI.gameObject.SetActive(true);
            timerUI.gameObject.SetActive(true);
            if (Runner.IsServer)
            {
                var players = Runner.ActivePlayers;
                foreach (var player in players)
                {
                    Vector3 spawnPosition =
                        new Vector3(3 + (player.RawEncoded % Runner.Config.Simulation.DefaultPlayers) * 5, -10.5f, 0);
                    NetworkObject networkObject = Runner.Spawn(_prefabRef, spawnPosition, Quaternion.identity, player);
                    RPC_UpdatePlayerList(networkObject.GetComponent<Player.Player>());
                }
            }
        }

        [Rpc]
        private void RPC_UpdatePlayerList(Player.Player player)
        {
            _players[player.Runner.LocalPlayer] = player;
            Debug.LogError(player.Object.Id);
            // if (player.Object.InputAuthority.PlayerId == Object.InputAuthority.PlayerId)
            // {
            Debug.LogError(player.Object.InputAuthority.PlayerId + " " + Object.InputAuthority.PlayerId);
            player.InitGameManager(this);
            // }
        }

        #endregion
    }
}