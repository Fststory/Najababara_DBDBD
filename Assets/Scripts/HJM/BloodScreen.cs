using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BloodScreenEffect : MonoBehaviour
{
    public GameObject bloodGo;   // ���� ��ũ�� ������Ʈ
    public Image bloodScreen;   // ���� ��ũ�� �̹��� (UI �̹���)
    public float fadeDuration = 1.0f;  // ���İ��� ����Ǵ� �� �ɸ��� �ð� (��)
    public bool isblood = false;

    private Coroutine currentCoroutine;

    private void Update()
    {
        if (bloodGo.activeSelf && !isblood)
        {
            // bloodGo�� Ȱ��ȭ�Ǿ���, ���� Ȱ��ȭ�� ���°� �ƴ϶�� ������ ��Ÿ���� ��
            ActivateBloodScreen();
            isblood = true;
        }
        else if (!bloodGo.activeSelf && isblood)
        {
            // bloodGo�� ��Ȱ��ȭ�Ǿ���, ���� Ȱ��ȭ�� ���¶�� ������ ������� ��
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
