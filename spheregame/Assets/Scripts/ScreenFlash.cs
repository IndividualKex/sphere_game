using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    public Image image;

    float flashTimer;

    void Update() {
        flashTimer = Mathf.Max(flashTimer - Time.deltaTime, 0);
        image.enabled = flashTimer > 0;
    }

    public void Flash() {
        flashTimer = .05f;
    }
}
