using Fusion;
using UnityEngine;

namespace VitaliyNULL.FusionManager
{
    public class GameManager : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private NetworkPrefabRef _prefabRef;

        #endregion

        public override void Spawned()
        {
            Debug.Log(Runner.SessionInfo.PlayerCount + "/" + Runner.SessionInfo.MaxPlayers + "Players now");
            if (Runner.SessionInfo.PlayerCount == Runner.SessionInfo.MaxPlayers)
            {
                RPC_SpawnAllPlayer();
            }
        }

        [Rpc]
        private void RPC_SpawnAllPlayer()
        {
            if (Runner.IsServer)
            {
                var players = Runner.ActivePlayers;
                foreach (var player in players)
                {
                    Vector3 spawnPosition =
                        new Vector3(3 + (player.RawEncoded % Runner.Config.Simulation.DefaultPlayers) * 5, -7, 0);
                    Runner.Spawn(_prefabRef, spawnPosition, Quaternion.identity, player);
                }
            }
        }
    }
}