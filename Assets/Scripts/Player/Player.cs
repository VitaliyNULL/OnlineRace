using Fusion;
using UnityEngine;
using VitaliyNULL.MapGeneration;

namespace VitaliyNULL.Player
{
    public class Player : NetworkBehaviour
    {
        #region MonoBehaviour Callbacks

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MapTile"))
            {
                Debug.Log("Triggered Enter");
                other.GetComponent<MapTile>().SetActiveNextTile();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("MapTile"))
            {
                Debug.Log("Triggered Exit");
                other.GetComponent<MapTile>().DisablePrevTile();
            }
        }

        #endregion
    }
}