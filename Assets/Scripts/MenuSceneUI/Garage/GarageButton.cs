using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.Garage
{
    public class GarageButton: MonoBehaviour
    {
        [SerializeField] private GameObject currentUI;
        [SerializeField] private GameObject garageUI;
        [SerializeField] private GameObject carsInGarageUI;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener((() =>
            {
                currentUI.SetActive(false);
                garageUI.SetActive(true);
                carsInGarageUI.SetActive(true);
            }));
        }
    }
}