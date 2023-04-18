using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VitaliyNULL.Props;

namespace VitaliyNULL.MapGeneration
{
    public class PropsGenerator : NetworkBehaviour
    {
        [SerializeField] private List<SpawnPoint> spawnPoints;
        [SerializeField] private List<Prop> props;

        private Dictionary<SpawnPoint, bool> _spawnedProps = new Dictionary<SpawnPoint, bool>();

        public void GenerateTileProps()
        {
            foreach (var point in spawnPoints)
            {
                _spawnedProps[point] = false;
            }

            int iterations = Random.Range(0, 4);
            for (int i = 0; i < iterations; i++)
            {
                SpawnPoint spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                while (_spawnedProps[spawnPoint])
                {
                    spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                }

                _spawnedProps[spawnPoint] = true;
                Prop prop = Runner.Spawn(props[Random.Range(0, props.Count)],
                    spawnPoint.transform.position, spawnPoint.gameObject.transform.rotation);
                prop.Object.transform.SetParent(transform);
            }
        }
    }
}