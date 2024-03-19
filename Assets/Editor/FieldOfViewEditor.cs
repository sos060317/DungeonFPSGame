using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))] // FieldOfView�� ���� Ŀ���� ������
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI() // GUI �׷��ִ� �Լ�
    {
        FieldOfView fov = (FieldOfView)target; // ����ȯ
        Handles.color = Color.white;           // ���� ��ȯ(���)
        // ������Ʈ�� ������ ������Ʈ �������� �� ����� ���̾��������� �׸�
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        // �þ߰� ���Ѽ��� ������ ����
        Vector3 viewAngle01 = DirectionFormAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFormAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow; // ���� ��ȯ(�����)
        // �÷��̾��� ��ġ���� ���� * ������ ��ŭ ���� ����
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);  
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

        if (fov.canSeePlayer) // �÷��̾ �þ߰� �ȿ� �ִٸ�
        {
            Handles.color = Color.green; // ���� ��ȯ(�ʷϻ�)

            // �� ��ġ���� �÷��̾��� ��ġ�� ���� �׸�
            Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
        }
    }

    // �þ߰� ���Ѽ��� ������ ���ϴ� �Լ�
    private Vector3 DirectionFormAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY; // �þ߰��� ���� += ���� ������Ʈ�� y���� ��

        // �þ߰��� ����(�þ߰� ǥ�� �Ҷ�, ���� ���� ����)
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
