using UnityEngine;

public class ParticleAccelerator : MonoBehaviour
{
    public Particle particlePrefab;
    public PlayerData playerData;
    public float interval = 1f;

    float timer;

    void Update() {
        if(playerData.ParticleAcceleratorCount == 0) return;
        timer += Time.deltaTime;
        if(timer >= interval) {
            timer -= interval;
            for(int i = 0; i < playerData.ParticleAcceleratorCount; i++)
                Shoot();
        }
    }

    void Shoot() {
        Vector3 dir = Random.insideUnitCircle.normalized;
        Particle particle = Instantiate(particlePrefab, transform.position, Quaternion.identity, Generator.CurrentBlock);
        int damage = Mathf.CeilToInt(playerData.ParticleAcceleratorDamage * playerData.DamageMultiplier);
        particle.Initialize(dir, damage, playerData.ParticleAcceleratorPassThrough);
    }
}
