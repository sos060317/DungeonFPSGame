using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity,     // 마우스 감도
                  sprintSpeed,          // 달리기 속도
                  walkSpeed,            // 걷기 속도
                  jumpForce,            // 점프 파워
                  smoothTime;           // 목적지까지 도착하는 시간

    [SerializeField]                    
    private GameObject cameraHolder;    // 카메라를 자식으로 가지고 있는 오브젝트

    private float verticalLookRotation; // Y축 카메라 각도
    private bool grounded;              // 땅을 밟고 있는지 아닌지를 확인
    private Vector3 smoothMoveVelocity; // 스무스 이동 저장 변수(?)
    private Vector3 moveAmount;         // 움직임 양

    private Rigidbody rigid;            // 강체 역학 컴포넌트

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();          // 강체 역학 컴포넌트 초기화

        Cursor.visible = false;                     // 마우스 커서를 안 보이게 함
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서를 고정시킴
    }

    private void Update()
    {
        Look();
        Move();
        Jump();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Look()
    {
        // 마우스가 수평으로 움직이면 플레이어의 Y축을 회전
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity); 

        // 마우스가 수직으로 움직이면 그 각도를 저장하고 그 각도를 -90 ~ 90으로 제한함(카메라 뒤집어짐을 방지)
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // 위에서 구한 카메라 수직 움직임을 적용시킴
        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    private void Move()
    {
        // 방향키 값을 저장하고 정규화함(목표지점)
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        // 목표지점까지 부드럽게 가는 벡터를 저장함
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed),
                     ref smoothMoveVelocity, smoothTime);
    }

    private void Movement()
    {
        // Move()에서 구한 값을 통해서 현재 위치에서 목표위치로 이동한다.
        rigid.MovePosition(rigid.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        // Space값을 눌렀을 때, grounded을 확인하고 조건에 만족하면 up쪽 방향으로 힘을 줌
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rigid.AddForce(transform.up * jumpForce);
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        // 플레이어가 현재 땅에 닿아있는지 아닌지를 확인함
        grounded = _grounded;
    }
}
