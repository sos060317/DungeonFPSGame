using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadInGame : MonoBehaviour
{
    private char[] leftAndRight = { 'L', 'R' };

    public void LoadScene()
    {
        SceneManager.LoadScene(SelectStartScene());
    }

    private string SelectStartScene()
    {
        int num = Random.Range(0, leftAndRight.Length);
        return "StartRoom" + leftAndRight[num];
    }
}
