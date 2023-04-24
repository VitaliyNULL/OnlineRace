using TMPro;
using UnityEngine;

namespace VitaliyNULL.InGameUI
{
    public class GameUI : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private TMP_Text _nickName;
        [SerializeField] private TMP_Text _distance;
        // [SerializeField] private TMP_Text _position;

        #endregion

        #region Public Methods

        public void UpdatePlayerNickName(string name)
        {
            _nickName.text = name;
        }

        public void UpdatePlayerDistance(float distance)
        {
            _distance.text = Mathf.RoundToInt(distance).ToString();
        }

        public void UpdatePlayerPosition(short position)
        {
            _distance.text = position.ToString();
        }

        #endregion
    }
}