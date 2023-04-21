using System.Collections;
using Fusion;
using UnityEngine;

namespace VitaliyNULL.MapGeneration
{
    public class MapTile : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private PropsGenerator _propsGenerator;
        private bool _isDisabling;

        #endregion

        #region Public Fields

        [HideInInspector] public MapTile previousMapTile;
        [HideInInspector] public MapTile nextMapTile;

        #endregion

        #region Public Methods

        public void StartChainToActiveTile(MapTile mapTile, int index)
        {
            if (mapTile.nextMapTile != null && index < 5)
            {
                if (mapTile.nextMapTile.isActiveAndEnabled)
                {
                    index++;
                    mapTile.StartChainToActiveTile(mapTile.nextMapTile, index);
                }
                else
                {
                    mapTile.RPC_SetActiveNextTile();
                }
            }
        }

        public void StartDisablingTile(MapTile mapTile, int index)
        {
            if (mapTile._isDisabling) return;
            mapTile.RPC_DisableTile();
        }

        /// <summary>
        /// Use this method when trigger enter tile for spawning next tile 
        /// </summary>
        private void SetActiveNextTile()
        {
            nextMapTile?.gameObject.SetActive(true);
            Debug.Log("Activate next tile");
        }

        /// <summary>
        /// Use this methods when trigger exit tile for disable prev tile
        /// </summary>
        private void DisablePrevTile()
        {
            previousMapTile?.gameObject.SetActive(false);
            Debug.Log("Disable prev tile");
        }

        private void DisableTile()
        {
            gameObject.SetActive(false);
            Debug.Log("Disable tile");
        }

        /// <summary>
        /// Generate Props on MapTile
        /// </summary>
        public void GenerateTile()
        {
            _propsGenerator.GenerateTileProps();
        }

        /// <summary>
        /// Generate Finish on MapTile
        /// </summary>
        public void GenerateFinish()
        {
            Debug.Log("Generated Finish");
        }

        /// <summary>
        /// Generate Start on MapTile
        /// </summary>
        public void GenerateStart()
        {
            Debug.Log("Generated Start");
        }

        #endregion

        #region Coroutines

        private IEnumerator WaitForDespawnTile()
        {
            _isDisabling = true;
            yield return new WaitForSeconds(20);
            DisableTile();
        }

        #endregion

        #region RPC

        [Rpc]
        private void RPC_SetActiveNextTile()
        {
            SetActiveNextTile();
        }


        [Rpc]
        private void RPC_DisableTile()
        {
            StartCoroutine(WaitForDespawnTile());
        }

        #endregion
    }
}