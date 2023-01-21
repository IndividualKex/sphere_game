using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class LevelUpMenu : MonoBehaviour
{
    public event UnityAction OnSelect;
    
    public Canvas canvas;
    public RectTransform contents;
    public PlayerData playerData;
    public UpgradeOption[] options;

    List<PlayerUpgrade> upgrades;
    Sequence sequence;
    bool active;

    void OnDisable() {
        sequence?.Kill();
    }

    public void Show() {
        if(active) return;
        upgrades = new List<PlayerUpgrade>();
        for(int i = 0; i < options.Length; i++) {
            PlayerUpgrade upgrade = playerData.GetAvailableUpgrade(upgrades);
            if(upgrade != null) {
                options[i].Initialize(upgrade, playerData.GetUpgradeLevel(upgrade) + 1);
                upgrades.Add(upgrade);
            } else {
                if(upgrades.Count == 0) {
                    OnSelect?.Invoke();
                    return;
                } else {
                    options[i].Hide();
                }
            }
        }
        active = true;
        sequence?.Kill();
        canvas.enabled = true;
        contents.anchoredPosition = Vector2.right * 1024f;
        sequence = DOTween.Sequence()
            .Append(contents.DOAnchorPosX(0, .3f))
            .SetEase(Ease.OutExpo)
            .SetUpdate(true);
    }

    public void Hide() {
        if(!active) return;
        sequence?.Kill();
        active = false;
        sequence = DOTween.Sequence()
            .Append(contents.DOAnchorPosX(1024f, .2f))
            .SetEase(Ease.InQuad)
            .SetUpdate(true)
            .OnComplete(() => canvas.enabled = false);
    }

    public void SelectOption(int option) {
        playerData.ApplyUpgrade(upgrades[option]);
        OnSelect?.Invoke();
        Hide();
    }
}
