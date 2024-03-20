using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyBase), true)] // FieldOfView�� ���� Ŀ���� ������, ���� Ŭ�������� ����
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI() // GUI �׷��ִ� �Լ�
    {
        EnemyBase fov = (EnemyBase)target; // ����ȯ
        Handles.color = Color.white;           // ���� ��ȯ(���)
        // ������Ʈ�� ������ ������Ʈ �������� �� ����� ���̾��������� �׸�
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.fieldOfView.lookRadius);

        Handles.color = Color.red;           // ���� ��ȯ(������)
        // ������Ʈ�� ������ ������Ʈ �������� �� ����� ���̾��������� �׸�
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.fieldOfView.noLookRadius);

        // �þ߰� ���Ѽ��� ������ ����
        Vector3 viewAngle01 = DirectionFormAngle(fov.transform.eulerAngles.y, -fov.fieldOfView.angle / 2);
        Vector3 viewAngle02 = DirectionFormAngle(fov.transform.eulerAngles.y, fov.fieldOfView.angle / 2);

        Handles.color = Color.yellow; // ���� ��ȯ(�����)
        // �÷��̾��� ��ġ���� ���� * ������ ��ŭ ���� ����
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.fieldOfView.lookRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.fieldOfView.lookRadius);

        if (fov.fieldOfView.canSeePlayer) // �÷��̾ �þ߰� �ȿ� �ִٸ�
        {
            Handles.color = Color.green; // ���� ��ȯ(�ʷϻ�)

            // �� ��ġ���� �÷��̾��� ��ġ�� ���� �׸�
            Handles.DrawLine(fov.transform.position, fov.fieldOfView.playerRef.transform.position);
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
