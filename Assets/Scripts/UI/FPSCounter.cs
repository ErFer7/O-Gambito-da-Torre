using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = gameObject.GetComponent<Text>();
    }

    void Update()
    {
        text.text = Mathf.Floor(1.0F / Time.unscaledDeltaTime) + " FPS";
    }
}
