using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TextButton : MonoBehaviour
{
    public RectTransform rect;

    Sequence sequence;

    void OnDisable() {
        sequence?.Kill();
    }

    public void HoverEnter() {
        sequence?.Kill();
        sequence = DOTween.Sequence()
            .Append(rect.DOScale(Vector3.one * 1.25f, .3f))
            .SetEase(Ease.OutExpo)
            .SetUpdate(true);
    }

    public void HoverExit() {
        sequence?.Kill();
        sequence = DOTween.Sequence()
            .Append(rect.DOScale(Vector3.one, .1f))
            .SetUpdate(true);
    }

    public void Click() {
        SceneManager.LoadScene(0);
    }
}
