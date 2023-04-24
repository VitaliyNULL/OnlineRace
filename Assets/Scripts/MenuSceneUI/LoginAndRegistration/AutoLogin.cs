using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class AutoLogin : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private FirebaseManager.FirebaseManager _firebaseManager;
        [SerializeField] private GameObject _loadingUI;
        [SerializeField] private GameObject _mainMenuUI;
        [SerializeField] private GameObject _registrationUI;

        #endregion


        #region MonoBehavior Callbacks

        private void Start()
        {
            if (PlayerPrefs.HasKey(ConstKeys.EmailKey))
            {
                _firebaseManager.InitializeFirebase();
                _firebaseManager.AutoLogin(PlayerPrefs.GetString(ConstKeys.EmailKey),
                    PlayerPrefs.GetString(ConstKeys.PasswordKey),
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