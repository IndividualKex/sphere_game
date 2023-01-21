using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PlayerData playerData;
    public MeshRenderer baseRenderer;
    public MeshRenderer flashRenderer;
    public GameObject deathPrefab;
    public HealthBar healthBar;

    int _health;
    int health {
        get {
            return _health;
        } set {
            _health = Mathf.Max(0, value);
        }
    }

    public int damage { get; private set; }

    Pickupable[] drops;
    float flashTimer;
    bool alive;

    public void Initialize(EnemyData data) {
        alive = true;
        health = data.maxHealth;
        baseRenderer.sharedMaterial = data.material;
        drops = data.drops;
        damage = data.damage;
        healthBar.Initialize(health, data.maxHealth);
    }

    void Update() {
        flashTimer = Mathf.Max(0, flashTimer - Time.deltaTime);
        flashRenderer.enabled = flashTimer > 0;
    }

    public bool Damage(int damage, bool silent = false) {
        if(!alive || damage == 0) return false;
        health -= damage;
        if(_health == 0) {
            Die();
        } else {
            healthBar.SetHealth(_health);
            if(!silent)
                Hit();
        }
        return !alive;
    }

    void Hit() {
        flashTimer = .1f;
    }

    void Die() {
        alive = false;
        Combo.IncrementCombo();
        Instantiate(deathPrefab, transform.position, Quaternion.identity, Generator.CurrentBlock);
        for(int i = 0; i < drops.Length; i++) {
            Vector3 pos = transform.position + Random.insideUnitSphere;
            pos.z = 0;
            Instantiate(drops[i], pos, Quaternion.identity, Generator.CurrentBlock);
        }
        playerData.RaiseEnemyKilled();
        Destroy(gameObject);
    }
}
