using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyBase), true)] // FieldOfView에 대한 커스텀 에디터, 하위 클래스에도 적용
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI() // GUI 그려주는 함수
    {
        EnemyBase fov = (EnemyBase)target; // 형변환
        Handles.color = Color.white;           // 색상 변환(흰색)
        // 컴포넌트가 부착된 오브젝트 기준으로 원 모양의 와이어프레임을 그림
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.fieldOfView.lookRadius);

        Handles.color = Color.red;           // 색상 변환(빨간색)
        // 컴포넌트가 부착된 오브젝트 기준으로 원 모양의 와이어프레임을 그림
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.fieldOfView.noLookRadius);

        // 시야각 제한선의 방향을 저장
        Vector3 viewAngle01 = DirectionFormAngle(fov.transform.eulerAngles.y, -fov.fieldOfView.angle / 2);
        Vector3 viewAngle02 = DirectionFormAngle(fov.transform.eulerAngles.y, fov.fieldOfView.angle / 2);

        Handles.color = Color.yellow; // 색상 변환(노란색)
        // 플레이어의 위치에서 방향 * 반지름 만큼 선을 그음
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.fieldOfView.lookRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.fieldOfView.lookRadius);

        if (fov.fieldOfView.canSeePlayer) // 플레이어가 시야각 안에 있다면
        {
            Handles.color = Color.green; // 색상 변환(초록색)

            // 내 위치에서 플레이어의 위치로 선을 그림
            Handles.DrawLine(fov.transform.position, fov.fieldOfView.playerRef.transform.position);
        }
    }

    // 시야각 제한선의 방향을 구하는 함수
    private Vector3 DirectionFormAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY; // 시야각의 절반 += 현재 오브젝트의 y각도 값

        // 시야각의 방향(시야각 표시 할때, 그을 선의 방향)
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
