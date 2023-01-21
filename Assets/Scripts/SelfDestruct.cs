using System.Collections;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float afterDelay = 5f;

    IEnumerator Start() {
        yield return new WaitForSeconds(afterDelay);
        Destroy(gameObject);
    }
}
