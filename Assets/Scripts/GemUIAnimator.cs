using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemUIAnimator : MonoBehaviour
{
    private RectTransform rt;
    private bool isAnimating = false;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void PlayCollectAnimation()
    {
        if (!isAnimating)
            StartCoroutine(AnimateGem());
    }

    private System.Collections.IEnumerator AnimateGem()
    {
        isAnimating = true;

        // ----- SCALE UP -----
        Vector3 originalScale = rt.localScale;
        Vector3 bigScale = originalScale * 1.4f;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 6f;
            rt.localScale = Vector3.Lerp(originalScale, bigScale, t);
            yield return null;
        }

        // ----- SCALE DOWN -----
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 6f;
            rt.localScale = Vector3.Lerp(bigScale, originalScale, t);
            yield return null;
        }

        isAnimating = false;
    }
}

