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
            gameObject.SetActive(true);
        }
    }
}