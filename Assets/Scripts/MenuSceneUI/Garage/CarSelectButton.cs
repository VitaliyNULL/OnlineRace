using UnityEngine;
using UnityEngine.UI;
using VitaliyNULL.Core;

namespace VitaliyNULL.MenuSceneUI.Garage
{
    public class CarSelectButton : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private GameObject _currentUI;
        [SerializeField] private GameObject _mainMenuUI;
        [SerializeField] private GameObject _carsInGarageUI;
        #endregion

        #region MyRegion

        [HideInInspector] public CarEnum carEnum = 0;

        #endregion

        public void SetCar(int index)
        {
            carEnum = (CarEnum)index;
        }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerPrefs.SetInt(ConstKeys.CarSkin, (int)carEnum);
                _currentUI.SetActive(false);
                _mainMenuUI.SetActive(true);
                _carsInGarageUI.SetActive(false);
                Debug.Log($"Current car with index {carEnum}");
            });
        }
    }
}