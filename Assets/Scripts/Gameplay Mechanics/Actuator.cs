using UnityEngine;

public class Actuator : MonoBehaviour
{
    public GameObject door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (door.activeSelf)
        {
            door.SetActive(false);
        }
        else
        {
            door.SetActive(true);
        }
    }
}
