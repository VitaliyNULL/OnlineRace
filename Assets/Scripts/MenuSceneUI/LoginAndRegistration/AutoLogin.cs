using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class AutoLogin : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private GameObject _loadingUI;
        [SerializeField] private GameObject _mainMenuUI;
        [SerializeField] private GameObject _registrationUI;

        #endregion


        #region MonoBehavior Callbacks

        private void Start()
        {
            if (PlayerPrefs.HasKey(ConstKeys.EmailKey))
            {
                FirebaseManager.FirebaseManager firebaseManager = FindObjectOfType<FirebaseManager.FirebaseManager>();
                firebaseManager.InitializeFirebase();
                firebaseManager.AutoLogin(PlayerPrefs.GetString(ConstKeys.EmailKey),
                    PlayerPrefs.GetString(ConstKeys.PasswordKey), OpenMainMenu, ErrorAuthorization);
            }
            else
            {
                _loadingUI.SetActive(false);
                _registrationUI.SetActive(true);
            }
        }

        #endregion

        #region Private Methods

        private void OpenMainMenu()
        {
            _loadingUI.SetActive(false);
            _mainMenuUI.SetActive(true);
        }

        private void ErrorAuthorization()
        {
            _loadingUI.SetActive(false);
            _registrationUI.SetActive(true);
        }

        #endregion
    }
}