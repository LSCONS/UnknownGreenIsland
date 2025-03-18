using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour
{
   public void OnClickEnter()
    {
        SceneManager.LoadScene("SihoonScene");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
