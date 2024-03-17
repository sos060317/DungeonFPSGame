using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity,     // ���콺 ����
                  sprintSpeed,          // �޸��� �ӵ�
                  walkSpeed,            // �ȱ� �ӵ�
                  jumpForce,            // ���� �Ŀ�
                  smoothTime;           // ���������� �����ϴ� �ð�

    [SerializeField]                    
    private GameObject cameraHolder;    // ī�޶� �ڽ����� ������ �ִ� ������Ʈ

    private float verticalLookRotation; // Y�� ī�޶� ����
    private bool grounded;              // ���� ��� �ִ��� �ƴ����� Ȯ��
    private Vector3 smoothMoveVelocity; // ������ �̵� ���� ����(?)
    private Vector3 moveAmount;         // ������ ��

    private Rigidbody rigid;            // ��ü ���� ������Ʈ

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();          // ��ü ���� ������Ʈ �ʱ�ȭ

        Cursor.visible = false;                     // ���콺 Ŀ���� �� ���̰� ��
        Cursor.lockState = CursorLockMode.Locked;   // ���콺 Ŀ���� ������Ŵ
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
        // ���콺�� �������� �����̸� �÷��̾��� Y���� ȸ��
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity); 

        // ���콺�� �������� �����̸� �� ������ �����ϰ� �� ������ -90 ~ 90���� ������(ī�޶� ���������� ����)
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // ������ ���� ī�޶� ���� �������� �����Ŵ
        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    private void Move()
    {
        // ����Ű ���� �����ϰ� ����ȭ��(��ǥ����)
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        // ��ǥ�������� �ε巴�� ���� ���͸� ������
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed),
                     ref smoothMoveVelocity, smoothTime);
    }

    private void Movement()
    {
        // Move()���� ���� ���� ���ؼ� ���� ��ġ���� ��ǥ��ġ�� �̵��Ѵ�.
        rigid.MovePosition(rigid.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        // Space���� ������ ��, grounded�� Ȯ���ϰ� ���ǿ� �����ϸ� up�� �������� ���� ��
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rigid.AddForce(transform.up * jumpForce);
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        // �÷��̾ ���� ���� ����ִ��� �ƴ����� Ȯ����
        grounded = _grounded;
    }
}
