using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour
{
    public GameObject optionUI;
   public void OnClickEnter()
    {
        SceneManager.LoadScene("SihoonScene");
    }

    public void OnClickOption()
    {
        optionUI.SetActive(!optionUI.activeSelf); // activeSelf를 활용하여 켜져 있는지 꺼져 있는지 확인함 
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
