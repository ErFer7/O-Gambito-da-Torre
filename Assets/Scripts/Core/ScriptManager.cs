using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptManager : MonoBehaviour
{
    public int maxLevel;

    private int level;

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

    private void Start()
    {
        level = 0;
    }

    public void AdvanceLevel()
    {
        if (level <= maxLevel)
        {
            SceneManager.LoadScene(++level);
        }
        else
        {
            level = 0;
            SceneManager.LoadScene(level);
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(level);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(0);
        level = 0;
    }

    // Método redundante
    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
        level = id;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
