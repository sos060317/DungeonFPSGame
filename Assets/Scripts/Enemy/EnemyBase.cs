using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public float radius;                          // 시야 반경
    [Range(0, 360)]
    public float angle;                           // 시야각

    public GameObject playerRef;                  // 플레이어 오브젝트

    [SerializeField]
    private LayerMask targetMask, obstructionMask; // 목표, 장애물(ex: 벽)

    public bool canSeePlayer;                     // 플레이어를 보고있는지 아닌지를 참거짓으로 확인

    protected virtual void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player"); // Tag가 "Player"인 오브젝트로 변수 초기화
        StartCoroutine(FOVRoutine()); // 코루틴 호출
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

    private void FieldOfViewCheck()
    {
        // radius값을 반지름으로 한 원 안에 특정 레이어(targetMask)를 가지고 있는 충돌체를 감지하여 배열에 저장함
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform; // 첫번째로 충돌된 충돌체의 위치를 저장함

            // 두 벡터의 위치 값을 빼고 정규화를 통해서 방향 벡터를 1로 하는 단위 벡터를 저장함
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // 내 전방과 directionToTarget사이의 값이 angle의 절반보다 작으면 if문 실행
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                // 내 위치와 target의 위치를 저장함
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // 현재 위치에서 directionToTarget의 방향으로 distanceToTarget거리 만큼을 검사하며
                // obstructionMask가 아니면 아마 targetMask이면 if문을 실행
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true; // 플레이어를 봄
                else
                    canSeePlayer = false; // 플레이어를 못 봄
            }
            else
                canSeePlayer = false; // 플레이어를 못 봄
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false; // 플레이어를 못 봄
        }
    }

    protected abstract void Move();
}
