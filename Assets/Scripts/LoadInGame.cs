using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadInGame : MonoBehaviour
{
    private char[] leftAndRight = { 'L', 'C', 'R' }; // 3종류의 씬의 마지막 철자를 가져옴(Left, Center, Right)

    public void LoadScene()
    {
        SceneManager.LoadScene(SelectStartScene()); // 씬을 로드함
    }

    private string SelectStartScene()
    {
        //3개의 철자를 하나 뽑고 그 철자를 "StartRoom"과 더하여 문자열을 완성 시키고 문자열에 해당하는 씬을 반환함
        int num = Random.Range(0, leftAndRight.Length);
        return "StartRoom" + leftAndRight[num];
    }
}
