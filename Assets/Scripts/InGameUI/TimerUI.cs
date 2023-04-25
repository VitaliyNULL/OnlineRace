using TMPro;
using UnityEngine;

namespace VitaliyNULL.InGameUI
{
    public class TimerUI: MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;

        public void ChangeTime(string time)
        {
            timerText.text = time;
        }
    }
}