using TMPro;
using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class UsernameInput: MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private GameObject _warningUI;

        #endregion

        #region Public Fields

        [HideInInspector] public string username = "";

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            GetComponent<TMP_InputField>().onValueChanged.AddListener(arg0 =>
            {
                username = arg0;
                _warningUI.SetActive(false);
            });
        }

        #endregion
    }
}