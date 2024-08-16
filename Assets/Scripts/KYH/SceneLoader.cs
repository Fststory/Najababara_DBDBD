using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // 다음 씬을 불러오는 함수
    public void LoadNextScene()
    {
        // 현재 씬의 빌드 인덱스를 가져옵니다.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 다음 씬의 빌드 인덱스를 계산합니다.
        int nextSceneIndex = currentSceneIndex + 1;

        // 다음 씬을 불러옵니다. (씬의 인덱스가 범위를 초과하지 않도록 체크)
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("더 이상 불러올 씬이 없습니다.");
        }
    }
}
