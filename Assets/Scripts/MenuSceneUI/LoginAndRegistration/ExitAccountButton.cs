using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class ExitAccountButton : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private GameObject _mainMenuUI;
        [SerializeField] private GameObject _loginUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => FindObjectOfType<FirebaseManager.FirebaseManager>().ExitAccount(
                () =>
                {
                    _mainMenuUI.SetActive(false);
                    _loginUI.SetActive(true);
                }));
        }

        #endregion
    }
}