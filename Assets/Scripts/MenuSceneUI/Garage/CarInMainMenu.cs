using System.Collections.Generic;
using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.Garage
{
    public class CarInMainMenu: MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private List<Car> _cars;
        private Car[] _carsForUse;
        private readonly string _carSkin = "CAR_SKIN";
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
            if (PlayerPrefs.HasKey(_carSkin))
            {
                _carsForUse[PlayerPrefs.GetInt(_carSkin)].gameObject.SetActive(true);
                Debug.Log($"Current car with index {PlayerPrefs.GetInt(_carSkin)}");
            }
            else
            {
                PlayerPrefs.SetInt(_carSkin, 0);
                _carsForUse[PlayerPrefs.GetInt(_carSkin)].gameObject.SetActive(true);
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

                _carsForUse[PlayerPrefs.GetInt(_carSkin)].gameObject.SetActive(true);
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