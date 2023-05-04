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
        [SerializeField] private GameObject _loadingUI;

        #endregion

        #region MonoBehaviour Callbacks

        #endregion

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                _registrationUI.gameObject.SetActive(false);
                _loadingUI.gameObject.SetActive(true);
                _warningUI.gameObject.SetActive(false);
                FirebaseManager.FirebaseManager firebaseManager = FindObjectOfType<FirebaseManager.FirebaseManager>();
                firebaseManager.InitializeFirebase();
                firebaseManager.RegisterButton(_emailInput, _passwordInput, _confirmPasswordInput, _usernameInput,
                    _warningUI, OpenMainMenu, OpenRegistrationMenu);
            });
        }

        private void OpenMainMenu()
        {
            _loadingUI.SetActive(false);
            _warningUI.gameObject.SetActive(false);
            _mainMenuUI.SetActive(true);
        }

        private void OpenRegistrationMenu()
        {
            _loadingUI.SetActive(false);
            _warningUI.gameObject.SetActive(false);
            _registrationUI.SetActive(true);
        }
    }
}