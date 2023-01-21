using UnityEngine;
using Cinemachine;

public class GameCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float shakeIntensity;

    CinemachineBasicMultiChannelPerlin noise;
    float shakeTimer;

    void Awake() {
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update() {
        shakeTimer = Mathf.Max(0, shakeTimer - Time.deltaTime);
        noise.m_AmplitudeGain = shakeIntensity * shakeTimer * shakeTimer;
    }

    public void Shake(float magnitude) {
        shakeTimer = magnitude;
    }
}
