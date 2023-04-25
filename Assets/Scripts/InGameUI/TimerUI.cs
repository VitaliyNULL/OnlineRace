using TMPro;
using UnityEngine;

namespace VitaliyNULL.InGameUI
{
    public class TimerUI: MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text startTimerText;

        public void ChangeTime(string time)
        {
            timerText.text = time;
        }

        public void ChangeStartTimer(string count)
        {
            startTimerText.text = count;
        }

        public void DisableStartTimer()
        {
            startTimerText.gameObject.SetActive(false);
        }
    }
}