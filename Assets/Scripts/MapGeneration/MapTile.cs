using Fusion;
using UnityEngine;

namespace VitaliyNULL.MapGeneration
{
    public class MapTile: NetworkBehaviour
    {
        [SerializeField] private PropsGenerator propsGenerator;
        public override void Spawned()
        {
            //TODO : Make random spawned objects on this tile
            propsGenerator.GenerateTileProps();
        }
        
    }
}