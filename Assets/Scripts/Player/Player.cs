using System.Collections;
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
        private MaterialChanger _materialChanger;
        private GameManager _gameManager;
        private bool _isInvulnerable;
        [SerializeField] public PlayerMove playerMove;

        #endregion

        #region Public Properties

        public bool IsInvulnerable
        {
            get => _isInvulnerable;
            set
            {
                _isInvulnerable = value;
                if (value)
                {
                    StartInvulnerability();
                }
            }
        }

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            _materialChanger = GetComponentInChildren<MaterialChanger>();
            if (HasInputAuthority)
            {
                CinemachineVirtualCamera Camera = FindObjectOfType<CinemachineVirtualCamera>();
                Transform target = _materialChanger.transform;
                Camera.Follow = target;
                Camera.LookAt = target;
            }
            else
            {
                //TODO: Change material to lowAlpha
                _materialChanger.ChangeMaterialToLowAlpha();
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
                mapTile.StartDisablingTile(mapTile, 0);
            }

            var colliders =
                Physics.OverlapBox(transform.position, new Vector3(3, 2, 7), Quaternion.identity, _propLayer);
            if (colliders != null)
            {
                // RPC_InteractWithProp(prop, this);
                foreach (var collider in colliders)
                {
                    collider.GetComponent<Prop>().Interact(this);
                }

                Debug.Log("Interact");
            }
        }

        #endregion

        #region Coroutines

        private IEnumerator WaitForInvulnerability()
        {
            yield return new WaitForSeconds(2f);
            IsInvulnerable = false;
        }

        #endregion

        #region Private Methods

        private void StartInvulnerability()
        {
            StartCoroutine(WaitForInvulnerability());
        }

        #endregion

        #region Public Methods

        public void InitGameManager(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        #endregion

        #region RPC

        // [Rpc]
        // private void RPC_InteractWithProp(Prop prop, Player player)
        // {
        //     if (HasStateAuthority)
        //     {
        //         prop.Interact(player);
        //         Debug.Log("Interacted");
        //     }
        // }

        #endregion
    }
}