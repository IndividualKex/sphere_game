using UnityEngine;
using TMPro;
using System;

public class GameTime : MonoBehaviour
{
    public static GameTime Instance { get; private set; }

    static float _time;
    static float time {
        get {
            return _time;
        } set {
            _time = value;
            TimeSpan timeSpan = new TimeSpan(0, 0, Mathf.FloorToInt(_time));
            Instance.text.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
    }
    public static float Value => time;

    public TextMeshProUGUI text;

    void Awake() {
        Instance = this;
        time = 0;
    }

    void Update() {
        time += Time.deltaTime;
    }
}
