using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class LoginButton : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private FirebaseManager.FirebaseManager _firebaseManager;
        [SerializeField] private EmailInput _emailInput;
        [SerializeField] private PasswordInput _passwordInput;
        [SerializeField] private WarningUI _warningUI;
        [SerializeField] private GameObject _loginUI;
        [SerializeField] private GameObject _mainMenuUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            GetComponent<Button>().onClick
                .AddListener(() =>
                {
                    _firebaseManager.InitializeFirebase();
                    _firebaseManager.LoginButton(_emailInput, _passwordInput, _warningUI, () =>
                    {
                        _loginUI.SetActive(false);
                        _mainMenuUI.SetActive(true);
                    });
                });
        }

        #endregion
    }
}