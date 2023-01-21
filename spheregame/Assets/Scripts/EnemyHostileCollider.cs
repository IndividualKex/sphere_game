using UnityEngine;

public class EnemyHostileCollider : MonoBehaviour
{
    public Enemy parent;
    public Collider col;

    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Player") {
            col.gameObject.GetComponentInParent<Player>().HitEnemy(parent);
            this.col.enabled = false;
        }
    }
}
