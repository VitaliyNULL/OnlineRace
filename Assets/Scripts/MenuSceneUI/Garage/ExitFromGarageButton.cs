using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.Garage
{
    public class ExitFromGarageButton : MonoBehaviour
    {
        [SerializeField] private GameObject currentUI;
        [SerializeField] private GameObject mainMenuUI;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener((() =>
            {
                currentUI.SetActive(false);
                mainMenuUI.SetActive(true);
            }));
        }
    }
}