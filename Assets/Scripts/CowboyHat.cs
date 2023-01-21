using UnityEngine;
using DG.Tweening;

public class CowboyHat : MonoBehaviour
{
    Sequence hopSequence;
    Sequence spinSequence;

    public void Hop() {
        hopSequence?.Kill();
        transform.localPosition = Vector3.zero;
        hopSequence = DOTween.Sequence()
            .Append(transform.DOLocalMoveY(.7f, .3f).SetEase(Ease.OutQuad))
            .Append(transform.DOLocalMoveY(0f, .2f).SetEase(Ease.InQuad));
    }

    public void Spin() {
        spinSequence?.Kill();
        transform.localRotation = Quaternion.identity;
        spinSequence = DOTween.Sequence()
            .Append(transform.DOLocalRotate(Vector3.up * 360f, .7f, RotateMode.FastBeyond360))
            .SetEase(Ease.OutQuad);
    }

    void OnDisable() {
        hopSequence?.Kill();
        spinSequence?.Kill();
    }
}
