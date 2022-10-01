using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{
    [SerializeField] private GameObject[] canvas;
    
    public void ActivateCanva(int canvaNumber)
    {
        foreach (var canva in canvas)
        {
            canva.SetActive(false);
        }
        canvas[canvaNumber].SetActive(true);
    }

    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
        Time.timeScale = 1;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
