using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BloodScreenEffect : MonoBehaviour
{
    public GameObject bloodGo;   // 블러드 스크린 오브젝트
    public Image bloodScreen;   // 블러드 스크린 이미지 (UI 이미지)
    public float fadeDuration = 1.0f;  // 알파값이 변경되는 데 걸리는 시간 (초)
    public bool isblood = false;

    private Coroutine currentCoroutine;

    private void Update()
    {
        if (bloodGo.activeSelf && !isblood)
        {
            // bloodGo가 활성화되었고, 현재 활성화된 상태가 아니라면 서서히 나타나게 함
            ActivateBloodScreen();
            isblood = true;
        }
        else if (!bloodGo.activeSelf && isblood)
        {
            // bloodGo가 비활성화되었고, 현재 활성화된 상태라면 서서히 사라지게 함
            DeactivateBloodScreen();
            isblood = false;
        }
    }

    public void ActivateBloodScreen()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(FadeToAlpha(1f));
    }

    public void DeactivateBloodScreen()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(FadeToAlpha(0f));
    }

    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        Color color = bloodScreen.color;
        float startAlpha = color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            color.a = newAlpha;
            bloodScreen.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        bloodScreen.color = color;
    }
}
