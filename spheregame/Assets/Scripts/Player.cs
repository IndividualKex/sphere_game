using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public UnityEvent onBeginMovingLeft;
    public UnityEvent onBeginMovingRight;
    public UnityEvent onJump;
    public UnityEvent onLand;
    public UnityEvent onPickUp;
    public UnityEvent onHitEnemy;
    public UnityEvent onDoubleJumpReady;
    public UnityEvent onDoubleJumpUnready;
    public UnityEvent onShoot;
    public UnityEvent onKill;

    public Canvas gameOver;
    public PlayerData playerData;
    public Experience experience;
    public HealthBar healthBar;
    public LineRenderer laserLineRenderer;
    public Transform groundCheck;
    public float groundCheckRadius = .2f;
    public float moveSpeed = 7f;
    public float jumpSpeed = 15f;
    public float acceleration = 50f;
    public float fallMultiplier = 5f;
    public float coyoteTime = .1f;
    public float laserDistance = 25f;
    public int maxHealth = 10;

    float _moveX;
    float moveX {
        get {
            return _moveX;
        } set {
            if(_moveX != value) {
                if(grounded) {
                    if(value > 0 && _moveX <= 0)
                        onBeginMovingRight?.Invoke();
                    else if(value < 0 && _moveX >= 0)
                        onBeginMovingLeft?.Invoke();
                }
                _moveX = value;
            }
        }
    }

    bool _grounded;
    bool grounded {
        get {
            return _grounded;
        } set {
            if(_grounded != value) {
                _grounded = value;
                if(_grounded) {
                    if(rigidBody.velocity.y <= 0)
                        onLand?.Invoke();
                    hasJumped = false;
                    hasDoubleJumped = false;
                    Combo.ResetCombo();
                } else {
                    prevUngroundedTime = Time.time;
                }
            }
        }
    }

    bool _doubleJumpReady;
    bool doubleJumpReady {
        get {
            return _doubleJumpReady;
        } set {
            if(_doubleJumpReady != value) {
                _doubleJumpReady = value;
                if(_doubleJumpReady)
                    onDoubleJumpReady?.Invoke();
                else
                    onDoubleJumpUnready?.Invoke();
            }
        }
    }

    int _health;
    int health {
        get {
            return _health;
        } set {
            _health = Mathf.Max(value, 0);
            if(_health == 0)
                Die();
            else
                healthBar.SetHealth(_health);
        }
    }

    bool acceptInputs => Time.timeScale > 0;

    Rigidbody rigidBody;
    int moveDir;
    float prevUngroundedTime;
    float laserTimer;
    bool hasJumped;
    bool hasDoubleJumped;

    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        hasDoubleJumped = true;
    }

    void Start() {
        playerData.Initialize();
        health = maxHealth;
        healthBar.Initialize(health, maxHealth);
        Time.timeScale = 1;
    }

    void OnEnable() {
        playerData.OnEnemyKilled += HandleEnemyKilled;
    }

    void OnDisable() {
        playerData.OnEnemyKilled -= HandleEnemyKilled;
    }

    void Update() {
        laserTimer = Mathf.Max(laserTimer - Time.deltaTime, 0);
        laserLineRenderer.enabled = laserTimer > 0;

        moveDir = acceptInputs ? (int)Input.GetAxisRaw("Horizontal") : 0;
        if(moveDir > 0)
            moveX = Mathf.MoveTowards(moveX, moveSpeed * playerData.MoveMultiplier, Time.deltaTime * acceleration * playerData.MoveMultiplier);
        else if(moveDir < 0)
            moveX = Mathf.MoveTowards(moveX, -moveSpeed * playerData.MoveMultiplier, Time.deltaTime * acceleration * playerData.MoveMultiplier);
        else
            moveX = Mathf.MoveTowards(moveX, 0, Time.deltaTime * acceleration * playerData.MoveMultiplier * 2f);

        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));
        doubleJumpReady = (!grounded && Time.time - prevUngroundedTime >= coyoteTime) && !hasDoubleJumped;

        if(Input.GetButtonDown("Jump") && acceptInputs) {
            if(!hasJumped && (grounded || Time.time - prevUngroundedTime < coyoteTime))
                Jump(false);
            else if(!hasDoubleJumped)
                Jump(true);
        }
    }

    void HandleEnemyKilled() {
        onKill?.Invoke();
        ResetDoubleJump();
    }

    public void Shoot() {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + Vector3.down * laserDistance;
        IEnumerable<RaycastHit> hits = Physics.RaycastAll(transform.position, Vector3.down, laserDistance, ~0, QueryTriggerInteraction.Collide).OrderBy(x => x.distance);
        int hitIdx = 0;
        int damage = Mathf.CeilToInt(playerData.LaserDamage * playerData.DamageMultiplier);
        List<Enemy> hitEnemies = new List<Enemy>();
        foreach(RaycastHit hit in hits) {
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
                if(hitEnemies.Contains(enemy))
                    continue;
                hitEnemies.Add(enemy);
                enemy.Damage(damage);
                if(++hitIdx > playerData.LaserPassThrough) {
                    endPos = new Vector3(transform.position.x, hit.transform.position.y, 0);
                    break;
                }
            } else if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                endPos = new Vector3(transform.position.x, hit.transform.position.y, 0);
                break;
            }
        }
        laserLineRenderer.SetPosition(0, startPos);
        laserLineRenderer.SetPosition(1, endPos);
        laserTimer = .1f;
        onShoot?.Invoke();
    }

    void Jump(bool doubleJump) {
        prevUngroundedTime = Time.time;
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpSpeed, 0);
        hasDoubleJumped = doubleJump;
        hasJumped = true;
        if(doubleJump)
            Shoot();
        else
            onJump?.Invoke();
    }

    void FixedUpdate() {
        if(rigidBody.velocity.y < jumpSpeed * .75f || (rigidBody.velocity.y > 0 && !Input.GetButton("Jump")))
            rigidBody.velocity += fallMultiplier * Physics.gravity.y * Vector3.up * Time.fixedDeltaTime;

        rigidBody.velocity = new Vector3(moveX, rigidBody.velocity.y, 0);
    }

    public void ResetDoubleJump(bool resetVelocity = false) {
        hasDoubleJumped = false;
        if(resetVelocity)
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, 0);
    }

    public void HitEnemy(Enemy enemy) {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, 0);
        Combo.ResetCombo();
        health -= enemy.damage;
        onHitEnemy?.Invoke();
    }

    void Die() {
        Time.timeScale = 0;
        gameOver.enabled = true;
    }

    public void PickUp(Pickupable pickupable) {
        if(pickupable is ExperiencePickupable) {
            int raw = ((ExperiencePickupable)pickupable).experience;
            int amount = raw * Mathf.Max(1, raw * Combo.Multiplier);
            experience.IncrementExperience(amount);
            onPickUp?.Invoke();
        } else {
            throw new System.NotImplementedException();
        }
    }
}
