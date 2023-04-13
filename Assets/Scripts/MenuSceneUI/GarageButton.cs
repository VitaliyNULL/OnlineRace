using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI
{
    public class GarageButton: MonoBehaviour
    {
        [SerializeField] private GameObject currentUI;
        [SerializeField] private GameObject garageUI;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener((() =>
            {
                currentUI.SetActive(false);
                garageUI.SetActive(true);
            }));
        }
    }
}