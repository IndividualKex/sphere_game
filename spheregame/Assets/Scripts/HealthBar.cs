using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public SpriteRenderer bar;
    public SpriteRenderer fill;
    public Transform pivot;

    int _health;
    int health {
        get {
            return _health;
        } set {
            if(_health != value) {
                _health = value;
                float v = Mathf.Clamp01(_health / (float)maxHealth);
                pivot.localScale = new Vector3(v, 1, 1);
            }
        }
    }

    int maxHealth;

    public void Initialize(int health, int maxHealth, bool visible = false) {
        this.maxHealth = maxHealth;
        this.health = health;
        if(visible)
            Show();
        else
            Hide();
    }

    public void Show() {
        bar.enabled = fill.enabled = true;
    }

    public void Hide() {
        bar.enabled = fill.enabled = false;
    }

    public void SetHealth(int health) {
        Show();
        this.health = health;
    }
}
