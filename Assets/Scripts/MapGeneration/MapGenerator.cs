using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace VitaliyNULL.MapGeneration
{
    public class MapGenerator : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private List<MapTile> _mapTiles;
        private List<MapTile> _spawnedTiles = new List<MapTile>();
        private MapTile _prevMapTile;
        private MapTile _currentMapTile;

        private MapTile _nextMapTile;

        //variables for generation tiles
        private readonly int _tilecount = 100;
        private readonly int _step = 60;
        private readonly int _startZ = -60;

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            if (Runner.SessionInfo.PlayerCount == Runner.SessionInfo.MaxPlayers)
            {
                Debug.Log("Generation Map");
                RPC_GenerateMap();
            }
        }

        #endregion

        #region Private Methods

        // /// <summary>
        // /// Use this method when spawning MapTile
        // /// </summary>
        // /// <param name="mapTile"> Object that spawned</param>
        // private void SetMapTileHierarchy(MapTile mapTile)
        // {
        //     if (_prevMapTile != null)
        //     {
        //         _prevMapTile.nextMapTile = _currentMapTile;
        //     }
        //
        //     if (_currentMapTile != null)
        //     {
        //         _prevMapTile = _currentMapTile;
        //         _currentMapTile = mapTile;
        //         _currentMapTile.previousMapTile = _prevMapTile; // prev !=null, current !=null, next == null
        //         return;
        //     }
        //
        //     _currentMapTile = mapTile;
        // }

        /// <summary>
        /// This method using only in init Game session
        /// </summary>
        private void GenerateMap()
        {
            int nextZ = _startZ;
            for (int i = 0; i < _tilecount; i++)
            {
                MapTile mapTile = Runner.Spawn(_mapTiles[0], new Vector3(0, -12, nextZ), Quaternion.identity);
                mapTile.GenerateTile();
                RPC_UpdateSpawnedTiles(mapTile);
                RPC_SetHierarchy(mapTile, _prevMapTile);
                RPC_RenameTile(mapTile, i);
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

            RPC_DisableSpawnedTiles();

            Debug.Log("a");
        }

        #endregion

        #region RPC

        [Rpc]
        private void RPC_UpdateSpawnedTiles(MapTile mapTile)
        {
            _spawnedTiles.Add(mapTile);
        }

        [Rpc]
        private void RPC_SetHierarchy(MapTile currentTile, MapTile prevTile)
        {
            if (_prevMapTile != null)
            {
                _prevMapTile.nextMapTile = currentTile;
            }

            if (_currentMapTile != null)
            {
                _prevMapTile = currentTile;
                _currentMapTile = currentTile;
                _currentMapTile.previousMapTile = prevTile; // prev !=null, current !=null, next == null
                return;
            }

            _currentMapTile = currentTile;
        }

        [Rpc]
        private void RPC_RenameTile(MapTile mapTile, int index)
        {
            mapTile.name = String.Format($"RoadTile {index}");
        }

        [Rpc]
        private void RPC_GenerateMap()
        {
            if (HasInputAuthority && HasStateAuthority)
                GenerateMap();
        }

        [Rpc]
        private void RPC_DisableSpawnedTiles()
        {
            if (_spawnedTiles == null) return;
            for (int i = 10; i < _tilecount; i++)
            {
                _spawnedTiles[i].gameObject.SetActive(false);
            }
        }

        #endregion
    }
}