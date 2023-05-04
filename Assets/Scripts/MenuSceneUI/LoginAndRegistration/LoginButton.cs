using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class LoginButton : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private EmailInput _emailInput;
        [SerializeField] private PasswordInput _passwordInput;
        [SerializeField] private WarningUI _warningUI;
        [SerializeField] private GameObject _loginUI;
        [SerializeField] private GameObject _mainMenuUI;
        [SerializeField] private GameObject _loadingUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            GetComponent<Button>().onClick
                .AddListener(() =>
                {
                    _loginUI.SetActive(false);
                    _warningUI.gameObject.SetActive(false);
                    _loadingUI.SetActive(true);
                    FirebaseManager.FirebaseManager firebaseManager =
                        FindObjectOfType<FirebaseManager.FirebaseManager>();
                    firebaseManager.InitializeFirebase();
                    firebaseManager.LoginButton(_emailInput, _passwordInput, _warningUI, OpenMainMenu, OpenLoginUI);
                });
        }

        #endregion

        private void OpenMainMenu()
        {
            _loadingUI.SetActive(false);
            _warningUI.gameObject.SetActive(false);
            _mainMenuUI.SetActive(true);
        }

        private void OpenLoginUI()
        {
            _loadingUI.SetActive(false);
            _warningUI.gameObject.SetActive(false);
            _loginUI.SetActive(true);
        }
    }
}