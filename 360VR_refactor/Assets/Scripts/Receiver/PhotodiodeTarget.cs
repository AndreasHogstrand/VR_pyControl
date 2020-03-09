using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotodiodeTarget : MonoBehaviour
{
    public Renderer renderer;

    /**
     * Used to show the photodiode target for 1 frame
     */
    public IEnumerator ShowTarget()
    {
        float timeoutCounter = 0.1f;

        renderer.enabled = true;

        while (timeoutCounter > 0)
        {
            timeoutCounter -= Time.deltaTime;
            yield return 0;
        }

        renderer.enabled = false;
        yield break;
    }

    // Start is called before the first frame update
    void Start()
    {
        renderer.enabled = false; ;
    }
}
