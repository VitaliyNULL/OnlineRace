using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace VitaliyNULL.FusionManager
{
    public class GameManager : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private NetworkPrefabRef _prefabRef;
        [SerializeField] private List<Player.Player> _players = new List<Player.Player>();

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            Debug.Log(Runner.SessionInfo.PlayerCount + "/" + Runner.SessionInfo.MaxPlayers + "Players now");
            if (Runner.SessionInfo.PlayerCount == Runner.SessionInfo.MaxPlayers)
            {
                RPC_SpawnAllPlayer();
            }
        }

        #endregion

        #region RPC

        [Rpc]
        private void RPC_SpawnAllPlayer()
        {
            if (Runner.IsServer)
            {
                var players = Runner.ActivePlayers;
                foreach (var player in players)
                {
                    Vector3 spawnPosition =
                        new Vector3(3 + (player.RawEncoded % Runner.Config.Simulation.DefaultPlayers) * 5, -10.5f, 0);
                    NetworkObject networkObject = Runner.Spawn(_prefabRef, spawnPosition, Quaternion.identity, player);
                    RPC_UpdatePlayerLis(networkObject.GetComponent<Player.Player>());
                }
            }
        }

        [Rpc]
        private void RPC_UpdatePlayerLis(Player.Player player)
        {
            _players.Add(player);
            if (player.Object.InputAuthority.PlayerId == Object.InputAuthority.PlayerId)
            {
                Debug.LogError(player.Object.InputAuthority.PlayerId + " " + Object.InputAuthority.PlayerId);
                player.InitGameManager(this);
            }
        }

        #endregion
    }
}