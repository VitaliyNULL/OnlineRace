using Fusion;
using UnityEngine;

namespace VitaliyNULL.MapGeneration
{
    public class MapTile : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private PropsGenerator _propsGenerator;

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

        /// <summary>
        /// Use this method when trigger enter tile for spawning next tile 
        /// </summary>
        public void SetActiveNextTile()
        {
            nextMapTile?.gameObject.SetActive(true);
            Debug.Log("Activate next tile");
        }

        /// <summary>
        /// Use this methods when trigger exit tile for disable prev tile
        /// </summary>
        public void DisablePrevTile()
        {
            previousMapTile?.gameObject.SetActive(false);
            Debug.Log("Disable prev tile");
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

        #region RPC

        [Rpc]
        private void RPC_SetActiveNextTile()
        {
            SetActiveNextTile();
        }

        #endregion
    }
}