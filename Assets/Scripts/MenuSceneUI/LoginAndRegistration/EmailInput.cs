using TMPro;
using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class EmailInput : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private GameObject _warningUI;

        #endregion

        #region Public Fields

        [HideInInspector] public string email = "";

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            GetComponent<TMP_InputField>().onValueChanged.AddListener(arg0 =>
            {
                email = arg0;
                _warningUI.SetActive(false);
            });
        }

        #endregion
    }
}