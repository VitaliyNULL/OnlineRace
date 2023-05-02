using TMPro;
using UnityEngine;

namespace VitaliyNULL.InGameUI
{
    public class EndRaceUI : MonoBehaviour
    {
        [SerializeField] private GameObject _gameUI;
        [SerializeField] private GameObject _timerUI;
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private TMP_Text _scoreText;

        public void ShowEndRaceUI()
        {
            _gameUI.SetActive(false);
            _timerUI.SetActive(false);
            gameObject.SetActive(true);
        }

        public void InitTimeAndScore(string time, string score)
        {
            _timeText.text = time;
            _scoreText.text = score;
        }
    }
}