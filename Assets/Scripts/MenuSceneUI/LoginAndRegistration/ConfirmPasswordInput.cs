using TMPro;
using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class ConfirmPasswordInput : MonoBehaviour
    {
        [HideInInspector] public string confirmPassword = "";
        [SerializeField] private GameObject warningUI;
        private void Start()
        {
            GetComponent<TMP_InputField>().onValueChanged.AddListener(arg0 =>
            {
                confirmPassword = arg0;
                warningUI.SetActive(false);
            });
        }
    }
}