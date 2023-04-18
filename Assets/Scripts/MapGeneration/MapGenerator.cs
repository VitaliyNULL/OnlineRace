using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace VitaliyNULL.MapGeneration
{
    public class MapGenerator : NetworkBehaviour
    {
        [SerializeField] private List<MapTile> mapTiles;

        private List<MapTile> _spawnedTiles = new List<MapTile>();
        private MapTile _prevMapTile;
        private MapTile _currentMapTile;
        private MapTile _nextMapTile;
        private readonly int _tilecount = 100;
        private readonly int _step = 60;
        private readonly int _startZ = -60;

        public override void Spawned()
        {
            GenerateMap();
        }

        private void SetMapTileHierarchy(MapTile mapTile)
        {
            if (_prevMapTile != null)
            {
                _prevMapTile.nextMapTile = _currentMapTile;
            }

            if (_currentMapTile != null)
            {
                _prevMapTile = _currentMapTile;
                _currentMapTile = mapTile;
                _currentMapTile.previousMapTile = _prevMapTile; // prev !=null, current !=null, next == null
                return;
            }

            _currentMapTile = mapTile;
        }

        private void GenerateMap()
        {
            int nextZ = _startZ;
            for (int i = 0; i < _tilecount; i++)
            {
                MapTile mapTile = Runner.Spawn(mapTiles[0], new Vector3(0, -12, nextZ), Quaternion.identity);
                _spawnedTiles.Add(mapTile);
                SetMapTileHierarchy(mapTile);
                mapTile.name = String.Format($"RoadTile {i}");
                if (i == 0)
                {
                    mapTile.GenerateStart();
                }
                else if (i == _tilecount - 10)
                {
                    mapTile.GenerateFinish();
                }
                else if (i > 6)
                {
                    mapTile.GenerateTile();
                }

                nextZ += _step;
            }

            for (int i = 10; i < _tilecount; i++)
            {
                _spawnedTiles[i].gameObject.SetActive(false);
            }

            Debug.Log("a");
        }
    }
}