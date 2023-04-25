using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VitaliyNULL.Props;

namespace VitaliyNULL.MapGeneration
{
    public class PropsGenerator : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private List<SpawnPoint> _spawnPoints;
        [SerializeField] private List<Prop> _props;
        private readonly Dictionary<SpawnPoint, bool> _spawnedProps = new Dictionary<SpawnPoint, bool>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Generate Random count of props
        /// </summary>
        public void GenerateTileProps()
        {
            foreach (var point in _spawnPoints)
            {
                _spawnedProps[point] = false;
            }

            int iterations = Random.Range(0, 4);
            for (int i = 0; i < iterations; i++)
            {
                SpawnPoint spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count - 1)];
                while (_spawnedProps[spawnPoint])
                {
                    spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count - 1)];
                }

                _spawnedProps[spawnPoint] = true;
                Prop prop = spawnPoint.SpawnProp(_props[Random.Range(0, _props.Count)]);
                _spawnPoints.Remove(spawnPoint);
                RPC_SetParent(prop);
            }
        }

        #endregion

        #region RPC

        [Rpc]
        private void RPC_SetParent(Prop prop)
        {
            prop?.Object.transform.SetParent(transform);
        }

        #endregion
    }
}