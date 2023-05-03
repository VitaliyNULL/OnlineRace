using System.Collections.Generic;
using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.MenuSceneUI.Garage
{
    public class CarInMainMenu: MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private List<Car> _cars;
        private Car[] _carsForUse;
        private bool _isInitialized ;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            _carsForUse = new Car[_cars.Count];
            foreach (var car in _cars)
            {
                _carsForUse[(int)car.carEnum] = car;
                car.gameObject.SetActive(false);
            }

            _isInitialized = true;
            if (PlayerPrefs.HasKey(ConstKeys.CarSkin))
            {
                _carsForUse[PlayerPrefs.GetInt(ConstKeys.CarSkin)].gameObject.SetActive(true);
                Debug.Log($"Current car with index {PlayerPrefs.GetInt(ConstKeys.CarSkin)}");
            }
            else
            {
                PlayerPrefs.SetInt(ConstKeys.CarSkin, 0);
                _carsForUse[PlayerPrefs.GetInt(ConstKeys.CarSkin)].gameObject.SetActive(true);
            }
        }

        private void OnEnable()
        {
            if (_isInitialized)
            {
                foreach (var car in _carsForUse)
                {
                    car.gameObject.SetActive(false);
                }

                _carsForUse[PlayerPrefs.GetInt(ConstKeys.CarSkin)].gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            foreach (var car in _carsForUse)
            {
                if (car == null) return;
                car.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}