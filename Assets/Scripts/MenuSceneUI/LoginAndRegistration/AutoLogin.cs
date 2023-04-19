using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class AutoLogin : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private FirebaseManager.FirebaseManager _firebaseManager;
        [SerializeField] private GameObject _loadingUI;
        [SerializeField] private GameObject _mainMenuUI;
        [SerializeField] private GameObject _registrationUI;
        private readonly string _passwordKey = "PASSWORD";
        private readonly string _emailKey = "EMAIL";

        #endregion


        #region MonoBehavior Callbacks

        private void Start()
        {
            if (PlayerPrefs.HasKey(_emailKey))
            {
                _firebaseManager.InitializeFirebase();
                _firebaseManager.AutoLogin(PlayerPrefs.GetString(_emailKey), PlayerPrefs.GetString(_passwordKey),
                    () =>
                    {
                        _loadingUI.SetActive(false);
                        _mainMenuUI.SetActive(true);
                    });
            }
            else
            {
                _loadingUI.SetActive(false);
                _registrationUI.SetActive(true);
            }
        }

        #endregion
    }
}