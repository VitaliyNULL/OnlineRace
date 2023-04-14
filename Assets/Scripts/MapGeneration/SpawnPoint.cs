using Fusion;
using UnityEngine;
using VitaliyNULL.Props;

namespace VitaliyNULL.MapGeneration
{
    public class SpawnPoint : NetworkBehaviour
    {
        public void SpawnProp(Prop prop)
        {
            Runner.Spawn(prop,transform.position,Quaternion.identity);
        }
    }
}