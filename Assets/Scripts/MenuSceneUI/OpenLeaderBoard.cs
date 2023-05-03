using UnityEngine;
using UnityEngine.UI;

namespace VitaliyNULL.MenuSceneUI.LeaderBoard
{
    public class OpenLeaderBoard : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenuUI;
        [SerializeField] private GameObject _leaderBoardUI;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                _mainMenuUI.SetActive(false);
                _leaderBoardUI.SetActive(true);
            });
        }
    }
}