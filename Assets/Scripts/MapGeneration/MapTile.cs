using Fusion;
using UnityEngine;

namespace VitaliyNULL.MapGeneration
{
    public class MapTile : NetworkBehaviour
    {
        [SerializeField] private PropsGenerator propsGenerator;
        public MapTile previousMapTile;
        public MapTile nextMapTile;

       

        public void SetActiveNextTile()
        {
            nextMapTile.gameObject.SetActive(true);
        }

        public void DisablePrevTile()
        {
            previousMapTile.gameObject.SetActive(false);
        }
        public void GenerateTile()
        {
            propsGenerator.GenerateTileProps();
        }

        public void GenerateFinish()
        {
            Debug.Log("Generated Finish");
        }

        public void GenerateStart()
        {
            Debug.Log("Generated Start");
        }
    }
}