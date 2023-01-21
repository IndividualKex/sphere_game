using System.Collections.Generic;
using UnityEngine;

public class Diffusion : MonoBehaviour
{
    public PlayerData playerData;
    public ParticleSystem particles;
    public float tickInterval = .1f;

    bool _active;
    bool active {
        get {
            return _active;
        } set {
            if(_active != value) {
                _active = value;
                if(_active)
                    particles.Play();
                else
                    particles.Stop();
            }
        }
    }

    float _radius;
    float radius {
        get {
            return _radius;
        } set {
            if(_radius != value) {
                _radius = value;
                ParticleSystem.MainModule main = particles.main;
                main.startSize = _radius * 2;
            }
        }
    }

    float timer;

    void Update() {
        active = playerData.DiffusionDamage > 0;
        radius = playerData.DiffusionRadius;
        if(!active) return;
        timer += Time.deltaTime;
        if(timer >= tickInterval) {
            timer -= tickInterval;
            Tick();
        }
    }

    void Tick() {
        List<Enemy> hit = new List<Enemy>();
        Collider[] col = Physics.OverlapSphere(transform.position, playerData.DiffusionRadius, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Collide);
        int damage = Mathf.CeilToInt(playerData.DiffusionDamage * playerData.DamageMultiplier);
        for(int i = 0; i < col.Length; i++) {
            Enemy enemy = col[i].transform.GetComponentInParent<Enemy>();
            if(hit.Contains(enemy))
                continue;
            hit.Add(enemy);
            enemy.Damage(damage, true);
        }
    }
}
