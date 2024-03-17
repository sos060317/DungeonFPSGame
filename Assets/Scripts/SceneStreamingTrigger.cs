using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStreamingTrigger : MonoBehaviour
{
    [SerializeField]
    private string streamTargetScene; // 비동기 로딩 할 씬

    private IEnumerator StreamingTargetScene()
    {
        // 씬 이름 저장
        var targetScene = SceneManager.GetSceneByName(streamTargetScene);
        if(!targetScene.isLoaded) // 이미 로딩이 되어있는지 확인
        {
            // 비동기 방식으로 씬 로딩
            var op = SceneManager.LoadSceneAsync(streamTargetScene, LoadSceneMode.Additive);

            while(!op.isDone) // 씬 로딩이 완료될때까지 대기
            {
                yield return null;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Player 충돌감지
        {
            // 플레이어가 나가 방향이 일정 각도보다 크다면 코루틴 실행
            var dir = Vector3.Angle(transform.forward, other.transform.position -transform.position);
            if(dir < 90)
            {
                StartCoroutine(StreamingTargetScene()); // 코루틴 호출
            }
        }
    }
}
