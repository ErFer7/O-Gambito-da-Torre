using UnityEngine;

public class GUIManager : MonoBehaviour
{
    private ScriptManager scriptManager;

    void Start()
    {
        scriptManager = GameObject.FindGameObjectWithTag("Script Manager").GetComponent<ScriptManager>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            scriptManager.LoadScene(0);
        }
        else if (Input.GetKey(KeyCode.R))
        {
            scriptManager.ResetLevel();
        }
    }

    public void ResetLevel()
    {
        scriptManager.ResetLevel();
    }
}
