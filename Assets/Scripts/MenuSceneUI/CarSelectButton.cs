using System.Collections.Generic;
using UnityEngine;

namespace VitaliyNULL.MenuSceneUI
{
    public class CarSelectButton : MonoBehaviour
    {
        private readonly string CAR_SKIN = "CAR_SKIN";
        private CarEnum _carEnum;

        public void SetCar(int index)
        {
            _carEnum = (CarEnum)index;
        }

        public void SelectCar()
        {
            PlayerPrefs.SetInt(CAR_SKIN,(int)_carEnum);
        }


    }
}