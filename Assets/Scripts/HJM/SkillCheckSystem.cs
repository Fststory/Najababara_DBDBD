using UnityEngine;

public class SkillCheckSystem : MonoBehaviour
{
    [HideInInspector]
    public GeneratorSystem generatorSystem; // 각 발전기의 GeneratorSystem을 참조
    public Transform noteAxis; // 스킬 체크 노트
    public Transform normalAxis; // 기준 노트 (정상 노트 위치를 나타냄)
    public GameObject noteCanvas; // 노트 캔버스 UI

    private float timer = 0.0f; // 스킬 체크 타이머
    private float rotationSpeed = 180.0f; // 노트의 회전 속도 (각도/초)
    private float maxRotationTime; // 노트가 두 바퀴 도는 데 걸리는 최대 시간
    private bool isChecking = false; // 스킬 체크 진행 중인지 여부

    private void Start()
    {
        noteCanvas.SetActive(false);
        maxRotationTime = 500.0f / rotationSpeed; // 두 바퀴 도는 데 걸리는 시간 계산
    }

    void Update()
    {
        if (isChecking)
        {
            // 타이머 업데이트
            timer += Time.deltaTime;

            // 스페이스바 입력 감지
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Check();
                print("스페이스바 눌림!");
            }

            // 두 바퀴 시간이 초과되면 실패 처리
            if (timer >= maxRotationTime)
            {
                FailCheck();
            }
        }
    }

    public void CheckStart()
    {
        noteCanvas.SetActive(true);
        generatorSystem.isSkillChecking = true; // 스킬 체크 시작 시 상태 변경
        timer = 0.0f; // 타이머 초기화
        isChecking = true; // 스킬 체크 진행 중 설정
    }

    public void Check()
    {
        // 스킬 체크 성공 여부 판단
        bool success = noteAxis.eulerAngles.z > 310 + normalAxis.eulerAngles.z || noteAxis.eulerAngles.z < 0 + normalAxis.eulerAngles.z;

        // 결과 처리
        generatorSystem.OnSkillCheckComplete(success);

        // 공통 처리
        noteCanvas.SetActive(false);
        isChecking = false; // 스킬 체크 진행 중 해제
    }

    private void FailCheck()
    {
        print("스킬 체크 시간 초과: 실패!!");
        generatorSystem.OnSkillCheckComplete(false);
        noteCanvas.SetActive(false);
        isChecking = false; // 스킬 체크 진행 중 해제
    }
}