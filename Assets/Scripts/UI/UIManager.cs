using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public float fadeTime;
    public CanvasGroup canvasGroup;

    private Coroutine coroutine_IFCA;
    private ScriptManager scriptManager;

    void Start()
    {
        scriptManager = GameObject.FindGameObjectWithTag("Script Manager").GetComponent<ScriptManager>();

        coroutine_IFCA = StartCoroutine(FadeCanvasAlpha(1.0F, fadeTime));
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Return)){

            PlayButton();
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            QuitButton();
        }
    }

    public void PlayButton()
    {
        scriptManager.LoadScene(1);
    }

    public void QuitButton()
    {
        scriptManager.Quit();
    }

    private IEnumerator FadeCanvasAlpha(float targetAlpha, float fadeTime)
    {
        float time = 0.0F;
        float initialAlpha = canvasGroup.alpha;

        do
        {
            canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, time / fadeTime);

            time += Time.unscaledDeltaTime;

            yield return null;

        } while (time < fadeTime);

        canvasGroup.alpha = targetAlpha;
    }
}
