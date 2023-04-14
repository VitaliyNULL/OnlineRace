using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.LoginAndRegistration
{
    public class ExitAccountButton : MonoBehaviour
    {
        [SerializeField] private FirebaseManager.FirebaseManager firebaseManager;
        [SerializeField] private GameObject mainMenuUI;
        [SerializeField] private GameObject loginUI;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => firebaseManager.ExitAccount(
                () =>
                {
                    mainMenuUI.SetActive(false);
                    loginUI.SetActive(true);
                }));
        }
    }
}