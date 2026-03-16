using UnityEngine;
using UnityEngine.SceneManagement;

public class LauncherManager : MonoBehaviour
{
    public void OnTap()
    {
        SceneManager.LoadScene("GameScene");
    }
}