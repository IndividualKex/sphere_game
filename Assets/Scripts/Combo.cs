using UnityEngine;
using TMPro;
using DG.Tweening;

public class Combo : MonoBehaviour
{
    public static Combo Instance { get; private set; }

    public TextMeshProUGUI text;
    public TextMeshProUGUI flash;
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI multiplierFlash;

    static int _combo;
    static int combo {
        get {
            return _combo;
        } set {
            if(_combo != value) {
                _combo = value;
                Instance.text.text = _combo.ToString();
                Instance.flash.text = _combo > 0 ? _combo.ToString() : "";
                multiplier = Mathf.CeilToInt(Mathf.Log(_combo + 2, 3f));
            }
        }
    }

    static int _multiplier;
    static int multiplier {
        get {
            return _multiplier;
        } set {
            if(_multiplier != value) {
                _multiplier = value;
                Instance.multiplierText.text = _multiplier > 1 ? _multiplier.ToString() + "x" : "";
                Instance.multiplierFlash.text = _multiplier > 1 ? _multiplier.ToString() + "x" : "";
            }
        }
    }
    public static int Multiplier => multiplier;

    static Sequence flashSequence;
    static Sequence multiplierFlashSequence;

    void Awake() {
        Instance = this;
    }

    void Start() {
        text.text = combo.ToString();
        multiplierText.text = "";
    }

    public static void IncrementCombo() {
        int prevMult = multiplier;
        combo++;
        flashSequence?.Kill();
        Instance.flash.transform.localScale = Vector3.one * 1.5f;
        flashSequence = DOTween.Sequence()
            .Append(Instance.flash.transform.DOScale(Vector3.one, .5f).SetEase(Ease.InQuad))
            .OnComplete(() => Instance.flash.text = "");
        if(multiplier > prevMult) {
            Instance.multiplierFlash.transform.localScale = Vector3.one * 1.5f;
            multiplierFlashSequence = DOTween.Sequence()
                .Append(Instance.multiplierFlash.transform.DOScale(Vector3.one, .5f).SetEase(Ease.InQuad))
                .OnComplete(() => Instance.multiplierFlash.text = "");
        }
    }

    public static void ResetCombo() {
        flashSequence?.Kill();
        multiplierFlashSequence?.Kill();
        combo = 0;
    }
}
