using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    bool allowInputs => Time.timeScale > 0;

    void Update() {
        if(Input.GetKeyDown(KeyCode.R) && allowInputs)
            SceneManager.LoadScene(0);
    }
}
