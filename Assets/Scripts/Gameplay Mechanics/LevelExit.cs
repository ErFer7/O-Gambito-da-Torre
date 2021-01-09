using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private ScriptManager scriptManager;

    private void Start()
    {
        scriptManager = GameObject.FindGameObjectWithTag("Script Manager").GetComponent<ScriptManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        scriptManager.AdvanceLevel();
    }
}
