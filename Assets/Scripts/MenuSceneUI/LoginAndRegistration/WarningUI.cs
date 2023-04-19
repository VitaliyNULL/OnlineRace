using TMPro;
using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class WarningUI : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private TMP_Text _warningText;

        #endregion

        #region Public Methods

        public void ChangeWarningText(string value)
        {
            _warningText.text = value;
            gameObject.SetActive(true);
        }

        #endregion
    }
}