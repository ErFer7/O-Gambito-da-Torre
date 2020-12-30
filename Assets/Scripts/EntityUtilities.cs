using UnityEngine;

public class EntityUtilities : MonoBehaviour
{

    // Esta função é possívelmente inútil agora. Mas ela poderá ser usada para evitar que a peça saia do "trilho" de movimento
    public static Vector2 AlignPosition(Vector2 vector)
    {
        float xFix = Mathf.Floor(vector.x / 0.5F) * 0.5F - vector.x;
        float yFix = Mathf.Floor(vector.y / 0.25F) * 0.25F - vector.y;

        Vector2 alignedVector = new Vector2(vector.x + xFix, vector.y + yFix);

        return alignedVector;
    }
}
