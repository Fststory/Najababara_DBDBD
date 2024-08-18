using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTriggerNextScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // ���� ���� ���� �ε����� �����ɴϴ�.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ���� ���� ���� �ε����� ����մϴ�.
        int nextSceneIndex = currentSceneIndex + 1;

        // ���� ���� �ҷ��ɴϴ�. (���� �ε����� ������ �ʰ����� �ʵ��� üũ)
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("�� �̻� �ҷ��� ���� �����ϴ�.");
        }
    }
}
