using UnityEngine;
using UnityEngine.EventSystems;


namespace VitaliyNULL.Player
{
    public class BrakePedal : MonoBehaviour
    {
        private void Start()
        {
            FusionManager.FusionManager fusionManager = FindObjectOfType<FusionManager.FusionManager>();
            EventTrigger eventTrigger = GetComponent<EventTrigger>();
            eventTrigger.triggers.Clear();
            EventTrigger.Entry pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((arg0 => { fusionManager._isBrakeDown = true; }));
            EventTrigger.Entry pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((arg0 => { fusionManager._isBrakeDown = false; }));
            eventTrigger.triggers.Add(pointerDown);
            eventTrigger.triggers.Add(pointerUp);
        }
    }
}