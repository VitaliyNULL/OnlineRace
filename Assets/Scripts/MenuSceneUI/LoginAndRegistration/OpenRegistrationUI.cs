using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class OpenRegistrationUI : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private GameObject _loginUI;
        [SerializeField] private GameObject _registrationUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                _loginUI.SetActive(false);
                _registrationUI.SetActive(true);
            });
        }

        #endregion
    }
}