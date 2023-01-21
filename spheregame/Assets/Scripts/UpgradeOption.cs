using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeOption : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI level;
    public TextMeshProUGUI description;
    public Image icon;

    PlayerUpgrade upgrade;

    public void Initialize(PlayerUpgrade upgrade, int level) {
        this.upgrade = upgrade;
        title.text = upgrade.displayName;
        this.level.text = "level: " + level.ToString();
        description.text = upgrade.descriptions[level - 1];
        icon.sprite = upgrade.sprite;
        Show();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
