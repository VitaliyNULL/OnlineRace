using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class OpenLoginUIButton : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private GameObject _registrationUI;
        [SerializeField] private GameObject _loginUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                _registrationUI.SetActive(false);
                _loginUI.SetActive(true);
            });
        }

        #endregion
    }
}