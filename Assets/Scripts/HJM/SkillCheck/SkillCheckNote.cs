using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckNote : MonoBehaviour
{
    public float rotationSpeed = 30.0f; // ȸ�� �ӵ�
    public Transform noteAxis; // ȸ�� ��
    public Transform note; // ȸ���ϴ� �̹��� UI

    private Quaternion startnoteRot; // ��Ʈ ȸ�� ����

    void Start()
    {
        if (note != null)
        {
            // note�� �ʱ� ȸ���� ��Ƶд�.
            startnoteRot = note.localRotation;
        }
    }

    void Update()
    {
        if (noteAxis != null && note != null)
        {
            // ȸ�� �ӵ��� ���
            float rotationThisFrame = rotationSpeed * Time.deltaTime;

            // noteAxis�� Z���� �������� ȸ��
            noteAxis.Rotate(Vector3.forward, rotationThisFrame, Space.Self);

            // note�� ȸ���� �ʱ� ���·� �ǵ�����.
            note.localRotation = startnoteRot;
        }
    }
}