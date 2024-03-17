using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStreamingTrigger : MonoBehaviour
{
    [SerializeField]
    private string streamTargetScene; // �񵿱� �ε� �� ��

    private IEnumerator StreamingTargetScene()
    {
        // �� �̸� ����
        var targetScene = SceneManager.GetSceneByName(streamTargetScene);
        if(!targetScene.isLoaded) // �̹� �ε��� �Ǿ��ִ��� Ȯ��
        {
            // �񵿱� ������� �� �ε�
            var op = SceneManager.LoadSceneAsync(streamTargetScene, LoadSceneMode.Additive);

            while(!op.isDone) // �� �ε��� �Ϸ�ɶ����� ���
            {
                yield return null;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Player �浹����
        {
            // �÷��̾ ���� ������ ���� �������� ũ�ٸ� �ڷ�ƾ ����
            var dir = Vector3.Angle(transform.forward, other.transform.position -transform.position);
            if(dir < 90)
            {
                StartCoroutine(StreamingTargetScene()); // �ڷ�ƾ ȣ��
            }
        }
    }
}
