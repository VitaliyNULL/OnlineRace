using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI
{
    public class StartRaceButton : MonoBehaviour
    {
        [SerializeField] private FusionManager.FusionManager fusionManager;
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject loadingUI;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                mainMenuUI.SetActive(false);
                loadingUI.SetActive(true);
                fusionManager.AutoPlay();
            });
        }
    }
}