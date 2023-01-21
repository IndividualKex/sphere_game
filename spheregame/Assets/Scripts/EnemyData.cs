using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public Pickupable[] drops;
    public Material material;
    public int maxHealth = 10;
    public int damage = 1;
}
