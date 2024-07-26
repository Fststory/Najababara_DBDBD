using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerMoveAlgorithm : MonoBehaviour
{
    // ų���� ������ �˰����� �����ϴ� ��ũ��Ʈ

    #region ���� ����
    /* ų���� ���������� �����ϸ� �յ��¿�� �̵��Ѵ�. �κ� û�ұ�ó��                  (+) �������� ������ �þ� ��ũ��Ʈ���� ����

        1. ų���� ó������ �̵��ϴ� ������ ���Ѵ�.
            1-1. ų���� ó�� �ٶ󺸴� ������ �������� ���Ѵ�. (Start �Լ����� ����)

        2. ų���� ȹ���� ���Ÿ� �������� �߰� ���¸� ���Ѵ�.
            2-1. ������ �̹߰�/ ������ �߰�/ ���� �߰�/ �Ҹ� ���� ��
            2-2. �� ���¸��� �ٸ� �������� ���δ�. (���¸��� �Լ� ����)
            2-3. �߰� ���´� �� ������ ������Ʈ �ǰ� ��ȯ�� �� �ִ�.
            2-4. �߰� ���� ������ �켱 ������ �ִ�. (0. �þ� �� ������/ 1. �þ� �� ���� o/ 2. �þ� �� �Ҹ� o <- �̷� ����)


    */
    #endregion

    #region SetStartLookDir() ����
    int startLookDir;   // ų���� ó�� �ٶ� ���� (���� ��[0 ~ 359]�� transform.eulerAngles.y�� ����)
    #endregion

    #region SetNowTracingState() ����
    #endregion

    void Start()
    {
        SetStartLookDir();  // ������ �� ������ ������ �ٶ󺻴�.
    }

    void Update()
    {
        SetNowTracingState();   // ���� �߰� ���¸� �����Ѵ�.
    }

    void SetStartLookDir()  // ó�� �ٶ󺸴� ������ ����
    {
        startLookDir = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, startLookDir, 0);
    }

    void SetNowTracingState()  // ���� �߰� ���¸� �����ϴ� �Լ� (1. ���� �� �켱������ �ű��.)
    {

    }



}
