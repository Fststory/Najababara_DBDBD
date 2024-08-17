
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadNextScene();
        }
    }

    // ���� ���� �ҷ����� �Լ�
    public void LoadNextScene()
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
