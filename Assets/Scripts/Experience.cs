using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
using System.Collections;

public class Experience : MonoBehaviour
{
    public UnityEvent onLevelUp;
    public Image fill;
    public TextMeshProUGUI levelText;
    public LevelUpMenu levelUpMenu;

    int _level;
    int level {
        get {
            return _level;
        } set {
            if(_level != value) {
                _level = value;
                levelText.text = string.Format("LV {0}", level);
            }
        }
    }

    int _experience;
    int experience {
        get {
            return _experience;
        } set {
            if(_experience != value) {
                _experience = value;
                fill.fillAmount = Mathf.Clamp01(experience / (float)required);
            }
        }
    }

    Sequence sequence;
    bool selected;
    int required;

    void Awake() {
        level = 1;
        experience = 0;
        required = GetExperienceRequired(level);
    }

    public void IncrementExperience(int amount) {
        experience += amount;
        CheckExperience();
    }

    void CheckExperience() {
        if(experience >= required) {
            int deduct = required;
            StartCoroutine(LevelUp(() => {
                required = GetExperienceRequired(level + 1);
                experience -= deduct;
                level++;
                CheckExperience();
            }));
        }
    }

    IEnumerator LevelUp(UnityAction callback) {
        Time.timeScale = 0;
        onLevelUp?.Invoke();
        selected = false;
        levelUpMenu.OnSelect += HandleSelect;
        levelUpMenu.Show();
        while(!selected) yield return null;
        Time.timeScale = 1;
        callback();
    }

    void HandleSelect() {
        levelUpMenu.OnSelect -= HandleSelect;
        selected = true;
    }

    int GetExperienceRequired(int level) {
        return Mathf.FloorToInt(Mathf.Pow(1.15f, level + 5) + 8 * Mathf.Pow((level - 1), 2.2f));
    }
}
