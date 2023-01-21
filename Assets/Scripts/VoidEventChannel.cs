using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEventChannel", menuName = "Scriptable Objects/Void Event Channel")]
public class VoidEventChannel : ScriptableObject
{
    public event UnityAction OnEventRaised;

    public void Raise() {
        OnEventRaised?.Invoke();
    }
}
