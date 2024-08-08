using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckNote : MonoBehaviour
{
    public float rotationSpeed = 30.0f; // 회전 속도
    public Transform noteAxis; // 회전 축
    public Transform note; // 회전하는 이미지 UI

    private Quaternion startnoteRot; // 노트 회전 저장

    void Start()
    {
        if (note != null)
        {
            // note의 초기 회전을 담아둔다.
            startnoteRot = note.localRotation;
        }
    }

    void Update()
    {
        if (noteAxis != null && note != null)
        {
            // 회전 속도를 계산
            float rotationThisFrame = rotationSpeed * Time.deltaTime;

            // noteAxis의 Z축을 기준으로 회전
            noteAxis.Rotate(Vector3.forward, rotationThisFrame, Space.Self);

            // note의 회전을 초기 상태로 되돌린다.
            note.localRotation = startnoteRot;
        }
    }
}