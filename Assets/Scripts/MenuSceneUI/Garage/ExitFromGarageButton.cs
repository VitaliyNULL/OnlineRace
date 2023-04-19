using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.Garage
{
    public class ExitFromGarageButton : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private GameObject _currentUI;
        [SerializeField] private GameObject _mainMenuUI;
        [SerializeField] private GameObject _carsInGarageUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener((() =>
            {
                _currentUI.SetActive(false);
                _carsInGarageUI.SetActive(false);
                _mainMenuUI.SetActive(true);
            }));
        }

        #endregion
    }
}