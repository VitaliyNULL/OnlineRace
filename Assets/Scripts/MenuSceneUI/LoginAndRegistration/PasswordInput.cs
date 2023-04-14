using TMPro;
using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class PasswordInput : MonoBehaviour
    {
        [HideInInspector]public string password = "";

        private void Start()
        {
            GetComponent<TMP_InputField>().onValueChanged.AddListener(arg0 => password = arg0);
        }
    }
}