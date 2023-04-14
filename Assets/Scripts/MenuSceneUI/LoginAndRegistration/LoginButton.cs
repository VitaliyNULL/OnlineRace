using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class LoginButton : MonoBehaviour
    {
        [SerializeField] private FirebaseManager.FirebaseManager firebaseManager;
        [SerializeField] private EmailInput emailInput;
        [SerializeField] private PasswordInput passwordInput;
        [SerializeField] private WarningUI warningUI;
        [SerializeField] private GameObject loginUI;
        [SerializeField] private GameObject mainMenuUI;

        private void Start()
        {
            GetComponent<Button>().onClick
                .AddListener(() =>
                {
                    firebaseManager.InitializeFirebase();
                    firebaseManager.LoginButton(emailInput, passwordInput, warningUI, () =>
                    {
                        loginUI.SetActive(false);
                        mainMenuUI.SetActive(true);
                    });
                });
        }
    }
}