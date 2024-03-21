using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    [System.Serializable]
    public struct FieldOfView
    {
        public float lookRadius;          // �þ߰� �ȿ� �־�� �����ϴ� �þ� �ݰ�
        public float noLookRadius;        // ���� ���� ���� ������ �׳� �����ع����� �þ� �ݰ�
        [Range(0, 360)]                   
        public float angle;               // �þ߰�
                                          
        [Space(10f)]                      
        public LayerMask targetMask;      // ��ǥ
        public LayerMask obstructionMask; // ��ֹ�(ex: ��)
                                          
        [HideInInspector]                 
        public bool canSeePlayer;         // �÷��̾ �����ִ��� �ƴ����� ���������� Ȯ��
                                          
        [HideInInspector]                 
        public GameObject playerRef;      // �÷��̾� ������Ʈ
    }

    [Header("Enemy Base")]
    public FieldOfView fieldOfView;       // Field of View ����ü

    private NavMeshAgent agent;           // NavMeshAgent ������Ʈ
    private Animator animator;            // Animator ������Ʈ

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent �ʱ�ȭ
        animator = GetComponent<Animator>();  // Animator     �ʱ�ȭ

        fieldOfView.playerRef = GameObject.FindGameObjectWithTag("Player"); // Tag�� "Player"�� ������Ʈ�� ���� �ʱ�ȭ

        StartCoroutine(FOVRoutine()); // �ڷ�ƾ ȣ��
    }

    protected abstract void Move();

    private void FieldOfViewCheck()
    {
        // radius���� ���������� �� �� �ȿ� Ư�� ���̾�(targetMask)�� ������ �ִ� �浹ü�� �����Ͽ� �迭�� ������
        Collider[] wideRangeChecks   = Physics.OverlapSphere(transform.position, fieldOfView.lookRadius, fieldOfView.targetMask);
        Collider[] narrowRangeChecks = Physics.OverlapSphere(transform.position, fieldOfView.noLookRadius, fieldOfView.targetMask);

        if (narrowRangeChecks.Length != 0) // ���� ������ �浹ü�� ������
        {
            agent.isStopped = false;                                 // agent Ȱ��ȭ
            Transform narrowTarget = narrowRangeChecks[0].transform; // ���� ������ ù��°�� �浹�� �浹ü�� ��ġ�� ������
            fieldOfView.canSeePlayer = true;                         // �÷��̾ ��
            animator.SetBool("canSeePlayer", true);                  // animator ���� ����
            agent.SetDestination(narrowTarget.position);             // wideTarget ���󰡱�
            LookPlayer();
        }
        else if (wideRangeChecks.Length != 0) // ���� ������ �浹ü�� ������
        {
            Transform wideTarget = wideRangeChecks[0].transform; // ���� ������ ù��°�� �浹�� �浹ü�� ��ġ�� ������

            // �� ������ ��ġ ���� ���� ����ȭ�� ���ؼ� ���� ���͸� 1�� �ϴ� ���� ���͸� ������
            Vector3 directionToTarget = (wideTarget.position - transform.position).normalized;

            // �� ����� directionToTarget������ ���� angle�� ���ݺ��� ������ if�� ����
            if (Vector3.Angle(transform.forward, directionToTarget) < fieldOfView.angle / 2)
            {
                // �� ��ġ�� target�� ��ġ�� ������
                float distanceToTarget = Vector3.Distance(transform.position, wideTarget.position);

                // ���� ��ġ���� directionToTarget�� �������� distanceToTarget�Ÿ� ��ŭ�� �˻��ϸ�
                // obstructionMask�� �ƴϸ� �Ƹ� targetMask�̸� if���� ����
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, fieldOfView.obstructionMask))
                {
                    agent.isStopped = false;                    // agent Ȱ��ȭ
                    fieldOfView.canSeePlayer = true;            // �÷��̾ ��
                    animator.SetBool("canSeePlayer", true);     // animator ���� ����
                    agent.SetDestination(wideTarget.position);  // wideTarget ���󰡱�
                }
                else
                {
                    fieldOfView.canSeePlayer = false; // �÷��̾ �� ��
                    animator.SetBool("canSeePlayer", false); // animator ���� ����
                    agent.isStopped = true;                  // agent ��Ȱ��ȭ
                }
            }
            else
            {
                fieldOfView.canSeePlayer = false;        // �÷��̾ �� ��
                animator.SetBool("canSeePlayer", false); // animator ���� ����
                agent.isStopped = true;                  // agent ��Ȱ��ȭ
            }
        }
        else if (fieldOfView.canSeePlayer)
        {
            fieldOfView.canSeePlayer = false;        // �÷��̾ �� ��
            animator.SetBool("canSeePlayer", false); // animator ���� ����
            agent.isStopped = true;                  // agent ��Ȱ��ȭ
        }
    }

    private void LookPlayer()
    {
        // �÷��̾� �ٶ󺸱�
        Vector3 targetDistance = fieldOfView.playerRef.transform.position - transform.position;
        targetDistance.y = 0f;

        if(targetDistance != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDistance);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 30f);
        }
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
}
