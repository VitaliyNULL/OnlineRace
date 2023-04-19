using TMPro;
using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class PasswordInput : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private GameObject _warningUI;

        #endregion

        #region Public Fields

        [HideInInspector] public string password = "";

        #endregion


        private void Start()
        {
            GetComponent<TMP_InputField>().onValueChanged.AddListener(arg0 =>
            {
                password = arg0;
                _warningUI.SetActive(false);
            });
        }
    }
}