using Fusion;
using UnityEngine;
using VitaliyNULL.Props;

namespace VitaliyNULL.MapGeneration
{
    public class SpawnPoint : NetworkBehaviour
    {
        #region Public Methods

        /// <summary>
        /// Method spawn prop in current spawn point
        /// </summary>
        /// <param name="prop">Object to spawn</param>
        /// <returns>Object that spawned</returns>
        public Prop SpawnProp(Prop prop)
        {
            return Runner.Spawn(prop, transform.position, Quaternion.identity);
        }

        #endregion
    }
}