using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField]
    private float radius;                          // �þ� �ݰ�

    [SerializeField, Range(0, 360)]
    private float angle;                           // �þ߰�

    [SerializeField]
    private GameObject playerRef;                  // �÷��̾� ������Ʈ

    [SerializeField]
    private LayerMask targetMask, obstructionMask; // ��ǥ, ��ֹ�(ex: ��)

    [SerializeField]
    private bool canSeePlayer;                     // �÷��̾ �����ִ��� �ƴ����� ���������� Ȯ��

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player"); // Tag�� "Player"�� ������Ʈ�� ���� �ʱ�ȭ
        StartCoroutine(FOVRoutine()); // �ڷ�ƾ ȣ��
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f); // 0.2�� ���� ����

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        // radius���� ���������� �� �� �ȿ� Ư�� ���̾�(targetMask)�� ������ �ִ� �浹ü�� �����Ͽ� �迭�� ������
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if(rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform; // ù��°�� �浹�� �浹ü�� ��ġ�� ������

            // �� ������ ��ġ ���� ���� ����ȭ�� ���ؼ� ���� ���͸� 1�� �ϴ� ���� ���͸� ������
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // �� ����� directionToTarget������ ���� angle�� ���ݺ��� ������ if�� ����
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                // �� ��ġ�� target�� ��ġ�� ������
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // ���� ��ġ���� directionToTarget�� �������� distanceToTarget�Ÿ� ��ŭ�� �˻��ϸ�
                // obstructionMask�� �ƴϸ� �Ƹ� targetMask�̸� if���� ����
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true; // �÷��̾ ��
                else
                    canSeePlayer = false; // �÷��̾ �� ��
            }
            else
                canSeePlayer = false; // �÷��̾ �� ��
        }
        else if(canSeePlayer)
        {
            canSeePlayer = false; // �÷��̾ �� ��
        }
    }
}
