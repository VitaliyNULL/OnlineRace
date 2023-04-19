using Cinemachine;
using Fusion;
using UnityEngine;
using VitaliyNULL.MapGeneration;

namespace VitaliyNULL.Player
{
    public class Player : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private LayerMask _layerMask;

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

        // private void OnCollisionEnter(Collision collision)
        // {
        //     Debug.LogError("CollisionEnter");
        //     if (collision.gameObject.layer == _layerMask)
        //     {
        //         Debug.Log("Triggered Enter");
        //     }
        // }
        //
        // private void OnCollisionExit(Collision other)
        // {
        //     Debug.LogError("CollisionExit");
        //
        //     if (other.gameObject.layer == _layerMask)
        //     {
        //         Debug.Log("Triggered Exit");
        //     }
        // }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == _layerMask)
            {
                Debug.Log("Triggered Enter");

                RPC_SetActiveNextTile(other.GetComponent<MapTile>());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == _layerMask)
            {
                Debug.Log("Triggered Exit");
                RPC_DisablePrevTile(other.GetComponent<MapTile>());
            }
        }

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

        #endregion
    }
}