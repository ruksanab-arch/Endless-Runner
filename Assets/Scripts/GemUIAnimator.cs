using UnityEngine;

public class GemUIAnimator : MonoBehaviour
{
    private RectTransform rect;
    private Vector3 originalScale;

    [Header("Animation Settings")]
    public float punchScale = 1.3f;
    public float animationSpeed = 0.2f;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        originalScale = rect.localScale;
    }

    public void PlayGemAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateGem());
    }

    private System.Collections.IEnumerator AnimateGem()
    {
        Vector3 bigScale = originalScale * punchScale;

        // Scale Up
        float timer = 0f;
        while (timer < animationSpeed)
        {
            rect.localScale = Vector3.Lerp(originalScale, bigScale, timer / animationSpeed);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // Scale Down
        timer = 0f;
        while (timer < animationSpeed)
        {
            rect.localScale = Vector3.Lerp(bigScale, originalScale, timer / animationSpeed);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        rect.localScale = originalScale;
    }
}
