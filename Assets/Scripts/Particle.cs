using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Particle : MonoBehaviour
{
    public UnityEvent onHit;
    public ParticleTrigger trigger;
    public float speed = 10f;

    List<Enemy> hitEnemies = new List<Enemy>();
    Rigidbody rigidBody;
    float startY;
    int passThrough;
    int damage;

    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
    }

    void OnEnable() {
        trigger.OnTrigger += HandleHit;
    }

    void OnDisable() {
        trigger.OnTrigger -= HandleHit;
    }

    void FixedUpdate() {
        rigidBody.velocity = rigidBody.velocity.normalized * speed;
        if(Mathf.Abs(transform.position.x) > 10 || Mathf.Abs(transform.position.y - startY) > Generator.Instance.blockHeight * 10)
            Destroy(gameObject);
    }

    public void Initialize(Vector3 dir, int damage, int passThrough) {
        rigidBody.velocity = dir * speed;
        this.damage = damage;
        this.passThrough = passThrough;
        startY = transform.position.y;
    }

    void HandleHit(Enemy enemy) {
        if(!hitEnemies.Contains(enemy)) {
            hitEnemies.Add(enemy);
            enemy.Damage(damage);
            onHit?.Invoke();
            if(hitEnemies.Count > passThrough)
                Destroy(gameObject);
        }
    }
}
