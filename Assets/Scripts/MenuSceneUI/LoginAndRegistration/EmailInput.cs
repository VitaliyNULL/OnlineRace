using TMPro;
using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class EmailInput : MonoBehaviour
    {
        [HideInInspector]public string email = "";
        private void Start()
        {
            GetComponent<TMP_InputField>().onValueChanged.AddListener(arg0 => email = arg0);
        }
    }
}