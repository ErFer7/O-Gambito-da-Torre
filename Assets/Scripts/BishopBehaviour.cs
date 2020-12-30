using System.Collections.Generic;
using UnityEngine;

public class BishopBehaviour : MonoBehaviour
{
    public float smoothTime;
    public float convergenceThreshold;
    public float sightDistance;
    public Player player;

    private Vector2 upDiagonal;
    private Vector2 downDiagonal;
    private Vector2 rightDiagonal;
    private Vector2 leftDiagonal;
    private Vector2 direction;
    private Vector2 targetTilePosition;
    private Vector2 velocity;
    private ContactFilter2D filter;
    private int raycastResultCount;
    private List<RaycastHit2D> raycastResults;
    private bool isMoving;
    private bool isTravelling;
    private int travelMoveCount;

    private void Start()
    {
        upDiagonal = new Vector2(0.0F, 0.5F);
        downDiagonal = new Vector2(0.0F, -0.5F);
        rightDiagonal = new Vector2(1.0F, 0.0F);
        leftDiagonal = new Vector2(-1.0F, 0.0F);
        velocity = Vector2.zero;

        filter = new ContactFilter2D();

        raycastResults = new List<RaycastHit2D>();

        isMoving = false;
        isTravelling = false;
    }

    private void FixedUpdate()
    {
        EntityUtilities.EnemyControl(ref isMoving,
                                     ref isTravelling,
                                     gameObject,
                                     ref targetTilePosition,
                                     ref velocity,
                                     smoothTime,
                                     convergenceThreshold);
    }

    private void Update()
    {
        EnemyBehaviour();
        EntityUtilities.Travel(ref isTravelling,
                               ref isMoving,
                               ref travelMoveCount,
                               gameObject.transform.position,
                               direction,
                               ref targetTilePosition);
    }

    private void EnemyBehaviour()
    {
        if (!isMoving && !isTravelling)
        {
            if (EntityUtilities.SearchForPlayer(upDiagonal,
                                                sightDistance,
                                                ref raycastResultCount,
                                                gameObject.transform.position,
                                                filter,
                                                ref raycastResults))
            {
                travelMoveCount = (int)((player.tilePosition.y - gameObject.transform.position.y) / upDiagonal.y);
                direction = upDiagonal;
                isTravelling = true;
            }
            else if (EntityUtilities.SearchForPlayer(downDiagonal,
                                                     sightDistance,
                                                     ref raycastResultCount,
                                                     gameObject.transform.position,
                                                     filter,
                                                     ref raycastResults))
            {
                travelMoveCount = (int)((player.tilePosition.y - gameObject.transform.position.y) / downDiagonal.y);
                direction = downDiagonal;
                isTravelling = true;
            }
            else if (EntityUtilities.SearchForPlayer(rightDiagonal,
                                                     sightDistance,
                                                     ref raycastResultCount,
                                                     gameObject.transform.position,
                                                     filter,
                                                     ref raycastResults))
            {
                travelMoveCount = (int)((player.tilePosition.x - gameObject.transform.position.x) / rightDiagonal.x);
                direction = rightDiagonal;
                isTravelling = true;
            }
            else if (EntityUtilities.SearchForPlayer(leftDiagonal,
                                                     sightDistance,
                                                     ref raycastResultCount,
                                                     gameObject.transform.position,
                                                     filter,
                                                     ref raycastResults))
            {
                travelMoveCount = (int)((player.tilePosition.x - gameObject.transform.position.x) / leftDiagonal.x);
                direction = leftDiagonal;
                isTravelling = true;
            }
        }
    }
}
