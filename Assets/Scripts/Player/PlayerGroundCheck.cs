using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    PlayerController playerController; // 플레이어 컨트롤러 컴포넌트

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>(); // 플레이어 컨트롤러 컴포넌트 초기화
    }

    #region 플레이어가 Trigger에 닿았거나, 닿지 않았거나, 닿아있거나를 확인
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

    #region 플레이어가 Collision에 닿았거나, 닿지 않았거나, 닿아있거나를 확인
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
