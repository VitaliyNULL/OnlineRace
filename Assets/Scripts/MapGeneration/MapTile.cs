using Fusion;
using UnityEngine;

namespace VitaliyNULL.MapGeneration
{
    public class MapTile : NetworkBehaviour
    {
        [SerializeField] private PropsGenerator propsGenerator;

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