using System;
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
        [HideInInspector] public EndRaceUI endRaceUI;
        private WaitingUI _waitingUI;
        [HideInInspector] public MapTile finishTile;

        [Networked] private bool _isGameStarted { get; set; }

        //7000 just for start iteration distance
        [Networked] private float _maxDistance { get; set; }

        //For updating pos

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

        public GameTime GetGameTime() => _gameTime;

        public void SetFinish(MapTile mapTile)
        {
            finishTile = mapTile;
        }

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            if (!HasInputAuthority) Runner.Despawn(Object);
            timerUI = FindObjectOfType<TimerUI>();
            _waitingUI = FindObjectOfType<WaitingUI>();
            gameUI = FindObjectOfType<GameUI>();
            endRaceUI = FindObjectOfType<EndRaceUI>();
            timerUI.gameObject.SetActive(false);
            gameUI.gameObject.SetActive(false);
            endRaceUI.gameObject.SetActive(false);
            Debug.Log(Runner.SessionInfo.PlayerCount + "/" + Runner.SessionInfo.MaxPlayers + "Players now");
            if (Runner.SessionInfo.PlayerCount == Runner.SessionInfo.MaxPlayers)
            {
                RPC_ChangeStartTimer();
                RPC_SpawnAllPlayer();
            }

            _maxDistance = 7000;
            _gameTime = new GameTime();
        }

        private void Update()
        {
            if (!IsGameStarted) return;
            foreach (var player in _players)
            {
                if (player.Value.playerMove.Distance == 0) return;
                if (_maxDistance > player.Value.playerMove.Distance)
                {
                    _maxDistance = player.Value.playerMove.Distance;
                    player.Value.CurrentPosition = 1;
                }
                else
                {
                    player.Value.CurrentPosition = 2;
                }
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (HasStateAuthority && IsGameStarted)
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
            _players[player.Object.InputAuthority] = player;
            // Debug.LogError(player.Object.Id);
            // if (player.Object.InputAuthority.PlayerId == Object.InputAuthority.PlayerId)
            // {
            // Debug.LogError(player.Object.InputAuthority.PlayerId + " " + Object.InputAuthority.PlayerId);
            player.InitGameManager(this);
            // }
        }

        #endregion
    }
}