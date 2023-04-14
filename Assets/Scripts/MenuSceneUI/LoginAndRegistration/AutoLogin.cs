using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class AutoLogin : MonoBehaviour
    {
        [SerializeField] private FirebaseManager.FirebaseManager firebaseManager;
        [SerializeField] private GameObject loadingUI;
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject registrationUI;
        private readonly string _passwordKey = "PASSWORD";
        private readonly string _emailKey = "EMAIL";

        private void Start()
        {
            if (PlayerPrefs.HasKey(_emailKey))
            {
                firebaseManager.InitializeFirebase();
                firebaseManager.AutoLogin(PlayerPrefs.GetString(_emailKey), PlayerPrefs.GetString(_passwordKey),
                    () =>
                    {
                        loadingUI.SetActive(false);
                        mainMenuUI.SetActive(true);
                    });
            }
            else
            {
                loadingUI.SetActive(false);
                registrationUI.SetActive(true);
            }
        }
    }
}