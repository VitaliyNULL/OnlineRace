using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.Garage
{
    public class CarSelectButton : MonoBehaviour
    {
        [SerializeField] private GameObject currentUI;
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject carsInGarageUI;
        private readonly string CAR_SKIN = "CAR_SKIN";
        [HideInInspector] public CarEnum carEnum = 0;

        public void SetCar(int index)
        {
            carEnum = (CarEnum)index;
        }

        private void Start()
        {
            
            GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerPrefs.SetInt(CAR_SKIN, (int)carEnum);
                currentUI.SetActive(false);
                mainMenuUI.SetActive(true);
                carsInGarageUI.SetActive(false);
                Debug.Log($"Current car with index {carEnum}");
            });
        }
    }
}