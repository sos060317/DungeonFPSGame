using System.Collections;
using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject openText; // 문열기 UI

    private float currentYAngle; // 현재 문의 각도

    private void Start()
    {
        openText.SetActive(false); // 비활성화 초기화
        currentYAngle = transform.rotation.eulerAngles.y; // 현재 각도 저장
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(openText != null)
                openText.SetActive(true); // 플레이어 접촉시 UI 활성화

            if(Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(OpenDoor(currentYAngle)); // 코루틴 호출
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (openText != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                openText.SetActive(false);  // UI 비활성화
            }
        }
    }

    private IEnumerator OpenDoor(float angle)
    {
        for(int i = 0; i < 120; i++)
        {
            transform.rotation = Quaternion.Euler(0, angle + i, 0);     // 현재 각도에서 120도 추가

            yield return null;
        }
        Destroy(openText);
    }
}
