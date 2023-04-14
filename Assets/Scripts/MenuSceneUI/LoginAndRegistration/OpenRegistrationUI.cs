using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class OpenRegistrationUI : MonoBehaviour
    {
        [SerializeField] private GameObject loginUI;
        [SerializeField] private GameObject registrationUI;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                loginUI.SetActive(false);
                registrationUI.SetActive(true);
            });
        }
    }
}