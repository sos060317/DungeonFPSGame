using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadInGame : MonoBehaviour
{
    private char[] leftAndRight = { 'L', 'C', 'R' }; // 3������ ���� ������ ö�ڸ� ������(Left, Center, Right)

    public void LoadScene()
    {
        SceneManager.LoadScene(SelectStartScene()); // ���� �ε���
    }

    private string SelectStartScene()
    {
        //3���� ö�ڸ� �ϳ� �̰� �� ö�ڸ� "StartRoom"�� ���Ͽ� ���ڿ��� �ϼ� ��Ű�� ���ڿ��� �ش��ϴ� ���� ��ȯ��
        int num = Random.Range(0, leftAndRight.Length);
        return "StartRoom" + leftAndRight[num];
    }
}
