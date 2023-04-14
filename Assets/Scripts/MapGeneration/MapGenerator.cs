using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace VitaliyNULL.MapGeneration
{
    public class MapGenerator : NetworkBehaviour
    {
        [SerializeField] private List<MapTile> mapTiles;

        private readonly int _tilecount = 100;
        private readonly int _step = 60;
        private readonly int _startZ = -60;

        public override void Spawned()
        {
            GenerateMap();
        }

        private void GenerateMap()
        {
            int nextZ = _startZ;
            for (int i = 0; i < _tilecount; i++)
            {
                Runner.Spawn(mapTiles[0], new Vector3(0, -12, nextZ), Quaternion.identity);
                nextZ += _step;
            }
        }
    }
}