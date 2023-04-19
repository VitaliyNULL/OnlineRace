using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class RegistrationButton : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private FirebaseManager.FirebaseManager _firebaseManager;
        [SerializeField] private EmailInput _emailInput;
        [SerializeField] private PasswordInput _passwordInput;
        [SerializeField] private ConfirmPasswordInput _confirmPasswordInput;
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
                _firebaseManager.InitializeFirebase();
                _firebaseManager.RegisterButton(_emailInput, _passwordInput, _confirmPasswordInput, _warningUI,
                    () =>
                    {
                        _registrationUI.SetActive(false);
                        _mainMenuUI.SetActive(true);
                    });
            });
        }
    }
}