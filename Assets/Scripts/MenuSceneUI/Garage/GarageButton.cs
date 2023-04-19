using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.Garage
{
    public class GarageButton : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private GameObject _currentUI;
        [SerializeField] private GameObject _garageUI;
        [SerializeField] private GameObject _carsInGarageUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener((() =>
            {
                _currentUI.SetActive(false);
                _garageUI.SetActive(true);
                _carsInGarageUI.SetActive(true);
            }));
        }

        #endregion


    }
}