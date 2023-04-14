using TMPro;
using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class WarningUI: MonoBehaviour
    {
        [SerializeField] private TMP_Text warningText;

        public void ChangeWarningText(string value)
        {
            warningText.text = value;
            warningText.gameObject.SetActive(true);

        }

        public void ClearWarningText()
        {
            warningText.text = "";
            warningText.gameObject.SetActive(false);
        }
    }
}