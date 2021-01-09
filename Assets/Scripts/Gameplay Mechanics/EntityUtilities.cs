using System.Collections.Generic;
using UnityEngine;

public class EntityUtilities : MonoBehaviour
{
    public static bool SearchForPlayer(Vector2 direction,
                                 float distance,
                                 ref int raycastResultCount,
                                 Vector3 position,
                                 ContactFilter2D filter,
                                 ref List<RaycastHit2D> raycastResults,
                                 GameObject gameObject)
    {
        bool playerFound = false;

        raycastResultCount = Physics2D.Raycast(position, direction, filter.NoFilter(), raycastResults, distance);

        for (int i = 0; i < raycastResultCount; i++)
        {
            if (raycastResults[i].collider.gameObject != gameObject)
            {
                if (raycastResults[i].collider.CompareTag("Player"))
                {
                    playerFound = true;
                    break;
                }
                else if (raycastResults[i].collider.CompareTag("Scenery") || raycastResults[i].collider.CompareTag("Enemy"))
                {
                    break;
                }
            }
        }

        return playerFound;
    }

    public static void Travel(ref bool isTravelling,
                              ref bool isMoving,
                              Vector3 position,
                              Vector2 direction,
                              ref int travelMoveCount,
                              ref Vector2 targetTilePosition)
    {
        if (isTravelling)
        {
            if (travelMoveCount > 0)
            {
                targetTilePosition = (Vector2)position + direction;
                isMoving = true;
                travelMoveCount--;
            }
            
            isTravelling = false;
        }
    }
    
    public static void EnemyControl(ref bool isMoving,
                                    ref bool isTravelling,
                                    GameObject gameObject,
                                    ref Vector2 targetTilePosition,
                                    ref Vector2 velocity,
                                    float smoothTime,
                                    float convergenceThreshold)
    {
        if (isMoving)
        {
            gameObject.transform.position = Vector2.SmoothDamp(gameObject.transform.position,
                                                               targetTilePosition,
                                                               ref velocity,
                                                               smoothTime);

            if (Vector2.Distance(gameObject.transform.position, targetTilePosition) < convergenceThreshold)
            {
                gameObject.transform.position = targetTilePosition;
                gameObject.transform.position = AlignPosition(gameObject.transform.position);
                isTravelling = true;
                isMoving = false;
            }
        }
    }

    public static void PlayerControl(ref bool isMoving,
                                     GameObject gameObject,
                                     ref Vector2 tilePosition,
                                     ref Vector2 velocity,
                                     float smoothTime,
                                     float convergenceThreshold)
    {
        if (isMoving)
        {
            gameObject.transform.position = Vector2.SmoothDamp(gameObject.transform.position,
                                                               tilePosition,
                                                               ref velocity,
                                                               smoothTime);

            if (Vector2.Distance(gameObject.transform.position, tilePosition) < convergenceThreshold)
            {
                gameObject.transform.position = tilePosition;
                gameObject.transform.position = AlignPosition(gameObject.transform.position);

                isMoving = false;
            }
        }
    }

    /* 
     * Esta função é possívelmente inútil agora. Mas ela poderá ser usada para evitar que a peça saia do "trilho" de 
     * movimento
     */
    private static Vector2 AlignPosition(Vector2 vector)
    {
        float xFix = Mathf.Floor(vector.x / 0.5F) * 0.5F - vector.x;
        float yFix = Mathf.Floor(vector.y / 0.25F) * 0.25F - vector.y;

        Vector2 alignedVector = new Vector2(vector.x + xFix, vector.y + yFix);

        return alignedVector;
    }
}
