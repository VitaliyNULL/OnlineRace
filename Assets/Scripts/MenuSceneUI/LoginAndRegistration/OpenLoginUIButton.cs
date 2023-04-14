using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class OpenLoginUIButton : MonoBehaviour
    {
        [SerializeField] private GameObject registrationUI;
        [SerializeField] private GameObject loginUI;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                registrationUI.SetActive(false);
                loginUI.SetActive(true);
            });
        }
    }
}