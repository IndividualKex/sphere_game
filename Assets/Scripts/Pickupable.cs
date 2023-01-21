using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public float attractSpeed = 50f;

    Rigidbody rigidBody;
    Player player;
    bool moveToPlayer;

    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other) {
        if(moveToPlayer) return;
        if(other.tag == "PickUp") {
            moveToPlayer = true;
            player = other.transform.GetComponentInParent<Player>();
        }
    }

    void FixedUpdate() {
        if(moveToPlayer) {
            rigidBody.position = Vector3.MoveTowards(transform.position, player.transform.position, attractSpeed * Time.fixedDeltaTime);
            if(Vector3.Distance(transform.position, player.transform.position) < .7f) {
                player.PickUp(this);
                Destroy(gameObject);
            }
        }
    }
}
