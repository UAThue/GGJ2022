using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HueEventSystem
{
    [System.Serializable]
    public class MessagePayload
    {
        public GameObject source; // This is who sent the message, if needed
        public GameObject target; // This is who the message is "aimed at", even if that is not the unit that received it (Remember: receiver == this )
        public int count; // How many -- useful for counting number of enemy deaths
        public string type; // What type -- useful for counting number of enemy deaths
    }

    [System.Serializable]
    public class PayloadUnityEvent : UnityEvent<MessagePayload>
    {
    }

    [System.Serializable]
    public class EventManager<TKey>
    {
        // Private Variables
        private Dictionary<TKey, PayloadUnityEvent> events;

        public EventManager()
        {
            events = new Dictionary<TKey, PayloadUnityEvent>();
        }

        public void TriggerEvent(TKey eventID, MessagePayload message = null)
        {
            if (events.ContainsKey(eventID) && events[eventID] != null) {
                events[eventID].Invoke(message);
            }
        }

        public void RegisterEvent(TKey eventID, UnityAction<MessagePayload> call)
        {
            if (!events.ContainsKey(eventID) || events[eventID] == null) {
                events[eventID] = new PayloadUnityEvent();
            }

            events[eventID].AddListener(call);
        }


        public void UnRegisterEvent(TKey eventID, UnityAction<MessagePayload> call)
        {
            if (events[eventID] != null) {
                events[eventID].RemoveListener(call);
            }
        }
    }
}