using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI
{
    public class StartRaceButton : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private FusionManager.FusionManager _fusionManager;
        [SerializeField] private GameObject _mainMenuUI;
        [SerializeField] private GameObject _loadingUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                _mainMenuUI.SetActive(false);
                _loadingUI.SetActive(true);
                _fusionManager.AutoPlay();
            });
        }

        #endregion
    }
}