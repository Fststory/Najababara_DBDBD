using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamearaMovement : MonoBehaviour
{
    public Transform objectToFollow; // ī�޶� ���� ���
    public float followSpeed = 10.0f; // ���󰡴� �ӵ�
    public float sensitivity = 100.0f; // ���콺 ����
    public float clampAngle = 70.0f; // ���� ����
    public float smoothness = 10.0f; // ī�޶��� �ε巯��
    //Time.deltaTime�� ������ ���� �������� �ε巴��, ���� Ŭ���� ������(��ĥ��)�̵���


    private float rotX; // ���콺 ��ǲ�� �޴� ����
    private float rotY; // ���콺 ��ǲ�� �޴� ����

    public Transform realCamera; // ����� ī�޶�
    public Vector3 dirNormalrized; // ���� 
    public Vector3 finaDir; // ���������� ������ ����
    public float minDistance; // ī�޶� ���ߴ� �ּҰŸ�
    public float maxDistance; // ī�޶� ���ߴ� �ִ�Ÿ�
    public float finalDistance; // ���������� ������ ī�޶�� ��� �� �Ÿ�
    // ���ع��� ���� �ÿ� ī�޶� �̵��ؼ� ��ġ�ϰ� �� �Ÿ�


    void Start()
    {
        // ���콺 ȸ������ �ʱ�ȭ �����ش�.
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;
        // ���Ͱ� �ʱ�ȭ �����ش�.
        dirNormalrized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;
        // Ŀ���� �÷��� ȭ�鿡 ����ش�.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;




    }


    void Update()
    {   // �� �����Ӹ��� ���콺�� ���� �޾ƿ� ������ �ð��� �����ش�.
        // Time.deltaTime ������ �����ӿ��� ���� �����ӱ����� �ð�����
        // ȭ����� ȸ���� XY�� ���콺 XY�� �ݴ���
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        // ȭ���� �Ѿ�� �ʰ� �������� ����(Clamp)�� �ɾ��ش�.
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        // ȸ������ ������ �׸� rot�� ���콺ȸ���� Quaternion.Euler(rotX, rotY, 0)�� �ִ´�. ���콺���� z�� ����(=0)
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    void LateUpdate() // Update�� ���� �� �����ϴ� �����ֱ� �Լ�
    {
        // ���󰡵��� �̵�(=position�� ���� �ٲ۴�.)
        // MoveTowards �Լ�: �� �� ���̸� ������ �ӵ��� �̵���Ű�� �� ���. 
        // ���͸� ���� �������� ���� �Ÿ���ŭ �̵���Ű���� �� ��
        // ����(ī�޶�) ��ġ���� -> ���� ����� ��ġ�� , �ӷ�(=�ӵ�*�ð�)
        transform.position = Vector3.MoveTowards(transform.position, objectToFollow.position, followSpeed * Time.deltaTime);


        // ���������� ������ �����ش�.
        // dirNormalrized�� ������ ī�޶� �ڽ��� ���� ��ǥ�� �ʱ�ȭ ���ѵ׾���
        // TransformPoint = ��ü�� ���� ��ǥ�踦 ���� ��ǥ��� ��ȯ(������ ���� �ʿ����)
        finaDir = transform.TransformPoint(dirNormalrized * maxDistance);

        // ���� ī�޶� ���̿� ���ع��� �����ϴ� �� �˻�
        // ���� Raycast(������, ����)�� ���ع��� �ε����ٸ�
        RaycastHit hit;
        if (Physics.Linecast(transform.position, finaDir, out hit, ~(1 << 11)))
        {
            // ���ع��� �ִٸ�, finalDistance(ī�޶�� ��� �����Ÿ�)��
            // hit.distance(Ray�� �浹�� ��ü������ �Ÿ�)�� minDistance�� maxDistance ������ ������ �����Ѵ�.
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else // �׷��� �ʰ�, �ε����� �ʾҴٸ�
        {
            // ���� �Ÿ��� �ִ� �Ÿ���,
            finalDistance = maxDistance;
        }
        // ī�޶��� ������ġ��
        // ī�޶��� ������ġ��
        // Vector3.Lerp(a, b, t): a�� b ���̸� t ������ŭ �����ϴ� �Լ�. 
        // t�� 0�� 1 ������ ������, 0�� a, 1�� b�� ��Ÿ����, �� ������ ���� �� �� ������ �߰����� ��ȯ
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalrized * finalDistance, Time.deltaTime * smoothness);
    }
}
// ī�޶� ���󰡰� �ϴ� �ڵ�: transform.position = Vector3.MoveTowards(...)�� ī�޶� ��� ������Ʈ�� ���󰡵��� �մϴ�.
// ī�޶��� ��ġ ����: realCamera.localPosition = Vector3.Lerp(...)�� ī�޶��� ���� ��ġ�� �ε巴�� �����Ͽ� ��ֹ��� ���� ������ ��ġ�� �����ϵ��� �մϴ�.