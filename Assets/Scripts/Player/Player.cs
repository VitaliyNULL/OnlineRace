using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Fusion;
using UnityEngine;
using VitaliyNULL.FusionManager;
using VitaliyNULL.MapGeneration;
using VitaliyNULL.Props;

namespace VitaliyNULL.Player
{
    public class Player : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private LayerMask _mapTileLayer;
        [SerializeField] private LayerMask _propLayer;
        private GameManager _gameManager;

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            if (HasInputAuthority)
            {
                CinemachineVirtualCamera Camera = FindObjectOfType<CinemachineVirtualCamera>();
                Camera.Follow = transform;
                Camera.LookAt = transform;
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, Vector3.down, out raycastHit, 3, _mapTileLayer))
            {
                MapTile mapTile = raycastHit.collider.GetComponent<MapTile>();
                mapTile.StartChainToActiveTile(mapTile, 0);
            }

            var colliders = Physics.OverlapBox(transform.position, new Vector3(3, 2, 7), Quaternion.identity,_propLayer);
            if (colliders != null)
            {
                var prop = colliders.SingleOrDefault().GetComponent<Prop>();
                
            }
        }

        #endregion

        #region Public Methods

        public void InitGameManager(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        #endregion
    }
}