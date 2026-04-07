using UnityEngine;
using TMPro;
using System.Collections;

public class QuestPopup : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI popupText;

    public float fadeSpeed = 2f;
    public float moveDistance = 50f;
    public float displayTime = 2f;

    private Coroutine currentCoroutine;
    private Vector3 originalPos;

    void Start()
    {
        canvasGroup.alpha = 0;
        originalPos = transform.localPosition;
    }

    public void ShowPopup(string message)
    {
        popupText.text = message;

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(AnimatePopup());
    }

    IEnumerator AnimatePopup()
    {
        Vector3 startPos = originalPos + new Vector3(0, moveDistance, 0);
        float t = 0;

        // 🔼 Move + Fade IN
        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;

            canvasGroup.alpha = t;
            transform.localPosition = Vector3.Lerp(startPos, originalPos, t);

            yield return null;
        }

        canvasGroup.alpha = 1;
        transform.localPosition = originalPos;

        // ⏸ Stay visible
        yield return new WaitForSeconds(displayTime);

        t = 1;

        // 🔽 Fade OUT
        while (t > 0)
        {
            t -= Time.deltaTime * fadeSpeed;

            canvasGroup.alpha = t;

            yield return null;
        }

        canvasGroup.alpha = 0;
    }
}
