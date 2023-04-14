using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI
{
    public class StartRaceButton : MonoBehaviour
    {
        [SerializeField] private FusionManager.FusionManager fusionManager;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => { fusionManager.AutoPlay(); });
        }
    }
}