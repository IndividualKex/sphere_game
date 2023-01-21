using UnityEngine;
using UnityEngine.Events;

public class GeneratorTrigger : MonoBehaviour
{
    public event UnityAction<GeneratorTrigger> OnTrigger;

    void OnTriggerEnter(Collider col) {
        if(col.gameObject.layer == LayerMask.NameToLayer("Player")) {
            OnTrigger?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
