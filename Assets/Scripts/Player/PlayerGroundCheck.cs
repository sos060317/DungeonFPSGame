using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    PlayerController playerController; // �÷��̾� ��Ʈ�ѷ� ������Ʈ

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>(); // �÷��̾� ��Ʈ�ѷ� ������Ʈ �ʱ�ȭ
    }

    #region �÷��̾ Trigger�� ��Ұų�, ���� �ʾҰų�, ����ְų��� Ȯ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(true);
    }
    #endregion

    #region �÷��̾ Collision�� ��Ұų�, ���� �ʾҰų�, ����ְų��� Ȯ��
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(true);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(true);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == playerController.gameObject)
            return;

        playerController.SetGroundedState(true);
    }
    #endregion
}
