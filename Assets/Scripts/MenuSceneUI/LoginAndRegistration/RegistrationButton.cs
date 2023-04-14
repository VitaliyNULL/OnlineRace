using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class RegistrationButton : MonoBehaviour
    {
        [SerializeField] private FirebaseManager.FirebaseManager firebaseManager;
        [SerializeField] private EmailInput emailInput;
        [SerializeField] private PasswordInput passwordInput;
        [SerializeField] private ConfirmPasswordInput confirmPasswordInput;
        [SerializeField] private WarningUI warningUI;
        [SerializeField] private GameObject registrationUI;
        [SerializeField] private GameObject mainMenuUI;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                firebaseManager.InitializeFirebase();
                firebaseManager.RegisterButton(emailInput, passwordInput, confirmPasswordInput, warningUI,
                    () =>
                    {
                        registrationUI.SetActive(false);
                        mainMenuUI.SetActive(true);
                    });
            });
        }
    }
}