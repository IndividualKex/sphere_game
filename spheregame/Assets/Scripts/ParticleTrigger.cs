using UnityEngine;
using UnityEngine.Events;

public class ParticleTrigger : MonoBehaviour
{
    public event UnityAction<Enemy> OnTrigger;

    void OnTriggerEnter(Collider col) {
        if(col.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            Enemy enemy = col.gameObject.GetComponentInParent<Enemy>();
            OnTrigger?.Invoke(enemy);
        }
    }
}
