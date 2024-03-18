using System.Collections;
using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject openText; // ������ UI

    private float currentYAngle; // ���� ���� ����

    private void Start()
    {
        openText.SetActive(false); // ��Ȱ��ȭ �ʱ�ȭ
        currentYAngle = transform.rotation.eulerAngles.y; // ���� ���� ����
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(openText != null)
                openText.SetActive(true); // �÷��̾� ���˽� UI Ȱ��ȭ

            if(Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(OpenDoor(currentYAngle)); // �ڷ�ƾ ȣ��
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (openText != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                openText.SetActive(false);  // UI ��Ȱ��ȭ
            }
        }
    }

    private IEnumerator OpenDoor(float angle)
    {
        for(int i = 0; i < 120; i++)
        {
            transform.rotation = Quaternion.Euler(0, angle + i, 0);     // ���� �������� 120�� �߰�

            yield return null;
        }
        Destroy(openText);
    }
}
