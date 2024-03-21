using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    [System.Serializable]
    public struct FieldOfView
    {
        public float lookRadius;          // 시야각 안에 있어야 감지하는 시야 반경
        public float noLookRadius;        // 일정 범위 내에 있으면 그냥 감지해버리는 시야 반경
        [Range(0, 360)]                   
        public float angle;               // 시야각
                                          
        [Space(10f)]                      
        public LayerMask targetMask;      // 목표
        public LayerMask obstructionMask; // 장애물(ex: 벽)
                                          
        [HideInInspector]                 
        public bool canSeePlayer;         // 플레이어를 보고있는지 아닌지를 참거짓으로 확인
                                          
        [HideInInspector]                 
        public GameObject playerRef;      // 플레이어 오브젝트
    }

    [Header("Enemy Base")]
    public FieldOfView fieldOfView;       // Field of View 구조체

    private NavMeshAgent agent;           // NavMeshAgent 컴포넌트
    private Animator animator;            // Animator 컴포넌트

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 초기화
        animator = GetComponent<Animator>();  // Animator     초기화

        fieldOfView.playerRef = GameObject.FindGameObjectWithTag("Player"); // Tag가 "Player"인 오브젝트로 변수 초기화

        StartCoroutine(FOVRoutine()); // 코루틴 호출
    }

    protected abstract void Move();

    private void FieldOfViewCheck()
    {
        // radius값을 반지름으로 한 원 안에 특정 레이어(targetMask)를 가지고 있는 충돌체를 감지하여 배열에 저장함
        Collider[] wideRangeChecks   = Physics.OverlapSphere(transform.position, fieldOfView.lookRadius, fieldOfView.targetMask);
        Collider[] narrowRangeChecks = Physics.OverlapSphere(transform.position, fieldOfView.noLookRadius, fieldOfView.targetMask);

        if (narrowRangeChecks.Length != 0) // 좁은 범위에 충돌체가 있으면
        {
            agent.isStopped = false;                                 // agent 활성화
            Transform narrowTarget = narrowRangeChecks[0].transform; // 좁은 범위에 첫번째로 충돌된 충돌체의 위치를 저장함
            fieldOfView.canSeePlayer = true;                         // 플레이어를 봄
            animator.SetBool("canSeePlayer", true);                  // animator 상태 변경
            agent.SetDestination(narrowTarget.position);             // wideTarget 따라가기
            LookPlayer();
        }
        else if (wideRangeChecks.Length != 0) // 넓은 범위에 충돌체가 있으면
        {
            Transform wideTarget = wideRangeChecks[0].transform; // 넓은 범위에 첫번째로 충돌된 충돌체의 위치를 저장함

            // 두 벡터의 위치 값을 빼고 정규화를 통해서 방향 벡터를 1로 하는 단위 벡터를 저장함
            Vector3 directionToTarget = (wideTarget.position - transform.position).normalized;

            // 내 전방과 directionToTarget사이의 값이 angle의 절반보다 작으면 if문 실행
            if (Vector3.Angle(transform.forward, directionToTarget) < fieldOfView.angle / 2)
            {
                // 내 위치와 target의 위치를 저장함
                float distanceToTarget = Vector3.Distance(transform.position, wideTarget.position);

                // 현재 위치에서 directionToTarget의 방향으로 distanceToTarget거리 만큼을 검사하며
                // obstructionMask가 아니면 아마 targetMask이면 if문을 실행
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, fieldOfView.obstructionMask))
                {
                    agent.isStopped = false;                    // agent 활성화
                    fieldOfView.canSeePlayer = true;            // 플레이어를 봄
                    animator.SetBool("canSeePlayer", true);     // animator 상태 변경
                    agent.SetDestination(wideTarget.position);  // wideTarget 따라가기
                }
                else
                {
                    fieldOfView.canSeePlayer = false; // 플레이어를 못 봄
                    animator.SetBool("canSeePlayer", false); // animator 상태 변경
                    agent.isStopped = true;                  // agent 비활성화
                }
            }
            else
            {
                fieldOfView.canSeePlayer = false;        // 플레이어를 못 봄
                animator.SetBool("canSeePlayer", false); // animator 상태 변경
                agent.isStopped = true;                  // agent 비활성화
            }
        }
        else if (fieldOfView.canSeePlayer)
        {
            fieldOfView.canSeePlayer = false;        // 플레이어를 못 봄
            animator.SetBool("canSeePlayer", false); // animator 상태 변경
            agent.isStopped = true;                  // agent 비활성화
        }
    }

    private void LookPlayer()
    {
        // 플레이어 바라보기
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
        WaitForSeconds wait = new WaitForSeconds(0.2f); // 0.2초 변수 저장

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
}
