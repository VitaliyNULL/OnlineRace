using System.Collections.Generic;
using Cinemachine;
using Fusion;
using UnityEngine;
using VitaliyNULL.FusionManager;
using VitaliyNULL.MapGeneration;

namespace VitaliyNULL.Player
{
    public class Player : NetworkBehaviour
    {
        #region Private Fields

        private List<MapTile> _tilesToDespawn = new List<MapTile>();
        private GameManager _gameManager;

        #endregion

        #region MonoBehaviour Callbacks

        public override void Spawned()
        {
            if (HasInputAuthority)
            {
                CinemachineVirtualCamera Camera = FindObjectOfType<CinemachineVirtualCamera>();
                Camera.Follow = transform;
                Camera.LookAt = transform;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("MapTile"))
            {
                Debug.Log("Triggered Enter");
                MapTile mapTile = other.GetComponent<MapTile>();
                RPC_SetActiveNextTile(mapTile);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("MapTile"))
            {
                Debug.Log("Triggered Exit");
                MapTile mapTile = other.GetComponent<MapTile>();
                _tilesToDespawn.Add(mapTile);
                // RPC_DisablePrevTile(mapTile);
            }
        }

        #endregion

        #region Public Methods

        public void InitGameManager(GameManager gameManager)
        {
            
        }

        #endregion
        #region RPC

        [Rpc]
        private void RPC_SetActiveNextTile(MapTile mapTile)
        {
            mapTile.SetActiveNextTile();
        }

        [Rpc]
        private void RPC_DisablePrevTile(MapTile mapTile)
        {
            mapTile.DisablePrevTile();
        }

        [Rpc]
        private void RPC_UpdateTilesToDespawn(MapTile mapTile)
        {
            
        }

        #endregion
    }
}