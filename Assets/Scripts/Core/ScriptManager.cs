using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptManager : MonoBehaviour
{
    private void Awake()
    {
        // Impede que duas intâncias da classe existam ao mesmo tempo (Singleton)
        if (GameObject.FindGameObjectsWithTag("Script Manager").Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
