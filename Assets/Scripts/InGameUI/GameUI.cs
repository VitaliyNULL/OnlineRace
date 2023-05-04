using TMPro;
using UnityEngine;

namespace VitaliyNULL.InGameUI
{
    public class GameUI : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private TMP_Text _distance;
        [SerializeField] private TMP_Text _position;

        #endregion

        #region Public Methods

        public void UpdatePlayerDistance(float distance)
        {
            _distance.text = Mathf.RoundToInt(distance).ToString();
        }

        public void UpdatePlayerPosition(short position)
        {
            _position.text = position.ToString();
        }

        #endregion
    }
}