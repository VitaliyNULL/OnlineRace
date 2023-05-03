using TMPro;
using UnityEngine;

namespace VitaliyNULL.MenuSceneUI.LeaderBoard
{
    public class LeaderBoardItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _username;
        [SerializeField] private TMP_Text _rating;

        public void Init(string username, string rating)
        {
            _username.text = username;
            _rating.text = rating;
        }
    }
}