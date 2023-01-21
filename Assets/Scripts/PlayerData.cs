using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Player Data")]
public class PlayerData : ScriptableObject
{
    public event UnityAction OnEnemyKilled;

    static readonly float BASE_DIFFUSION_RADIUS = 7f;
    
    [SerializeField] PlayerUpgrade[] availableUpgrades;

    public float DiffusionRadius { get; private set; }
    public float MoveMultiplier { get; private set; }
    public float DamageMultiplier { get; private set; }
    public int LaserPassThrough { get; private set; }
    public int LaserDamage { get; private set; }
    public int DiffusionDamage { get; private set; }
    public int ParticleAcceleratorCount { get; private set; }
    public int ParticleAcceleratorDamage { get; private set; }
    public int ParticleAcceleratorPassThrough { get; private set; }

    Dictionary<PlayerUpgrade, int> upgrades;

    public void Initialize() {
        upgrades = new Dictionary<PlayerUpgrade, int>();
        for(int i = 0; i < availableUpgrades.Length; i++)
            upgrades.Add(availableUpgrades[i], 0);
        LaserDamage = 10;
        LaserPassThrough = 0;
        DiffusionDamage = 0;
        DiffusionRadius = BASE_DIFFUSION_RADIUS;
        ParticleAcceleratorCount = 0;
        ParticleAcceleratorDamage = 10;
        ParticleAcceleratorPassThrough = 0;
        MoveMultiplier = 1f;
        DamageMultiplier = 1f;
    }

    public PlayerUpgrade GetAvailableUpgrade(List<PlayerUpgrade> excluding) {
        return availableUpgrades
            .Where(x => !excluding.Contains(x))
            .Where(x => upgrades[x] < x.descriptions.Length)
            .OrderBy(x => Random.Range(0f, 1f))
            .FirstOrDefault();
    }

    public void ApplyUpgrade(PlayerUpgrade upgrade) {
        upgrades[upgrade]++;
        switch(upgrade.name) {
            case "Laser":
                switch(upgrades[upgrade]) {
                    case 1:
                        LaserDamage = 10;
                        LaserPassThrough = 1;
                        break;
                    case 2:
                        LaserDamage = 20;
                        LaserPassThrough = 1;
                        break;
                    case 3:
                        LaserDamage = 20;
                        LaserPassThrough = 2;
                        break;
                    case 4:
                        LaserDamage = 30;
                        LaserPassThrough = 2;
                        break;
                    case 5:
                        LaserDamage = 30;
                        LaserPassThrough = 3;
                        break;
                    case 6:
                        LaserDamage = 45;
                        LaserPassThrough = 3;
                        break;
                }
                break;
            case "Diffusion":
                switch(upgrades[upgrade]) {
                    case 1:
                        DiffusionDamage = 1;
                        DiffusionRadius = BASE_DIFFUSION_RADIUS;
                        break;
                    case 2:
                        DiffusionDamage = 1;
                        DiffusionRadius = BASE_DIFFUSION_RADIUS * 1.4f;
                        break;
                    case 3:
                        DiffusionDamage = 2;
                        DiffusionRadius = BASE_DIFFUSION_RADIUS * 1.4f;
                        break;
                    case 4:
                        DiffusionDamage = 2;
                        DiffusionRadius = BASE_DIFFUSION_RADIUS * 1.6f;
                        break;
                    case 5:
                        DiffusionDamage = 3;
                        DiffusionRadius = BASE_DIFFUSION_RADIUS * 1.6f;
                        break;
                    case 6:
                        DiffusionDamage = 3;
                        DiffusionRadius = BASE_DIFFUSION_RADIUS * 1.8f;
                        break;
                    case 7:
                        DiffusionDamage = 4;
                        DiffusionRadius = BASE_DIFFUSION_RADIUS * 1.8f;
                        break;
                }
                break;
            case "ParticleAccelerator":
                switch(upgrades[upgrade]) {
                    case 1:
                        ParticleAcceleratorCount = 1;
                        ParticleAcceleratorDamage = 10;
                        ParticleAcceleratorPassThrough = 0;
                        break;
                    case 2:
                        ParticleAcceleratorCount = 2;
                        ParticleAcceleratorDamage = 11;
                        ParticleAcceleratorPassThrough = 0;
                        break;
                    case 3:
                        ParticleAcceleratorCount = 2;
                        ParticleAcceleratorDamage = 12;
                        ParticleAcceleratorPassThrough = 1;
                        break;
                    case 4:
                        ParticleAcceleratorCount = 3;
                        ParticleAcceleratorDamage = 13;
                        ParticleAcceleratorPassThrough = 1;
                        break;
                    case 5:
                        ParticleAcceleratorCount = 3;
                        ParticleAcceleratorDamage = 14;
                        ParticleAcceleratorPassThrough = 2;
                        break;
                    case 6:
                        ParticleAcceleratorCount = 4;
                        ParticleAcceleratorDamage = 15;
                        ParticleAcceleratorPassThrough = 2;
                        break;
                    case 7:
                        ParticleAcceleratorCount = 4;
                        ParticleAcceleratorDamage = 20;
                        ParticleAcceleratorPassThrough = 2;
                        break;
                }
                break;
            case "Swiftness":
                switch(upgrades[upgrade]) {
                    case 1:
                        MoveMultiplier = 1.2f;
                        break;
                    case 2:
                        MoveMultiplier = 1.4f;
                        break;
                    case 3:
                        MoveMultiplier = 1.6f;
                        break;
                    case 4:
                        MoveMultiplier = 1.8f;
                        break;
                    case 5:
                        MoveMultiplier = 2f;
                        break;
                }
                break;
            case "Empower":
                switch(upgrades[upgrade]) {
                    case 1:
                        DamageMultiplier = 1.2f;
                        break;
                    case 2:
                        DamageMultiplier = 1.4f;
                        break;
                    case 3:
                        DamageMultiplier = 1.6f;
                        break;
                    case 4:
                        DamageMultiplier = 1.8f;
                        break;
                    case 5:
                        DamageMultiplier = 2f;
                        break;
                }
                break;
        }
    }

    public int GetUpgradeLevel(PlayerUpgrade upgrade) {
        return upgrades[upgrade];
    }

    public void RaiseEnemyKilled() {
        OnEnemyKilled?.Invoke();
    }
}
