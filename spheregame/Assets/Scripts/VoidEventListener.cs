using UnityEngine;
using UnityEngine.Events;

public class VoidEventListener : MonoBehaviour
{
    public VoidEventChannel channel;
    public UnityEvent onEventRaised;

    void OnEnable() {
        channel.OnEventRaised += HandleEventRaised;
    }

    void OnDisable() {
        channel.OnEventRaised -= HandleEventRaised;
    }

    void HandleEventRaised() {
        onEventRaised?.Invoke();
    }
}
