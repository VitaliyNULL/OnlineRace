using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.LeaderBoard
{
    public class ExitFromLeaderBoard : MonoBehaviour
    {
        [SerializeField] private GameObject _leaderBoardUI;
        [SerializeField] private GameObject _mainMenuUI;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                _leaderBoardUI.SetActive(false);
                _mainMenuUI.SetActive(true);
            });
        }
    }
}