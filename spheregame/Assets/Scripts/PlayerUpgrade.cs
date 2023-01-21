using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgrade", menuName = "Scriptable Objects/Player Upgrade")]
public class PlayerUpgrade : ScriptableObject
{
    public string displayName;
    public string[] descriptions;
    public Sprite sprite;
}
