using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class RegistrationButton : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private EmailInput _emailInput;
        [SerializeField] private PasswordInput _passwordInput;
        [SerializeField] private ConfirmPasswordInput _confirmPasswordInput;
        [SerializeField] private UsernameInput _usernameInput;
        [SerializeField] private WarningUI _warningUI;
        [SerializeField] private GameObject _registrationUI;
        [SerializeField] private GameObject _mainMenuUI;

        #endregion

        #region MonoBehaviour Callbacks

        

        #endregion
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                FirebaseManager.FirebaseManager firebaseManager = FindObjectOfType<FirebaseManager.FirebaseManager>();
                firebaseManager.InitializeFirebase();
                firebaseManager.RegisterButton(_emailInput, _passwordInput, _confirmPasswordInput, _usernameInput, _warningUI,
                    () =>
                    {
                        _registrationUI.SetActive(false);
                        _mainMenuUI.SetActive(true);
                    });
            });
        }
    }
}