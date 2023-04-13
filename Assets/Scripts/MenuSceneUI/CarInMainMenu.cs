using System.Collections.Generic;
using UnityEngine;

namespace VitaliyNULL.MenuSceneUI
{
    public class CarInMainMenu: MonoBehaviour
    {
        [SerializeField] private List<Car> _cars;
        private Car[] _carsForUse;
        private readonly string CAR_SKIN = "CAR_SKIN";
        private bool _isInitialized = false;

        private void Start()
        {
            _carsForUse = new Car[_cars.Count];
            foreach (var car in _cars)
            {
                _carsForUse[(int)car.carEnum] = car;
                car.gameObject.SetActive(false);
            }
            _isInitialized = true;
            if (PlayerPrefs.HasKey(CAR_SKIN))
            {
                _carsForUse[PlayerPrefs.GetInt(CAR_SKIN)].gameObject.SetActive(true);
                Debug.Log($"Current car with index {PlayerPrefs.GetInt(CAR_SKIN)}");
            }
            else
            {
                PlayerPrefs.SetInt(CAR_SKIN,0);
                _carsForUse[PlayerPrefs.GetInt(CAR_SKIN)].gameObject.SetActive(true);
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

                _carsForUse[PlayerPrefs.GetInt(CAR_SKIN)].gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            foreach (var car in _carsForUse)
            {
                car.gameObject.SetActive(false);
            }
        }
    }
}