using Fusion;
using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.InGameUI
{
    public class ExitRaceButton: MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                FindObjectOfType<NetworkRunner>().Shutdown();
            });
        }
    }
}