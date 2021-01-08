using System.Collections.Generic;
using UnityEngine;

public class RoyalBehaviour : MonoBehaviour
{
    public float smoothTime;
    public float convergenceThreshold;
    public float sightDistance;
    public float verticalDiagonalSightDistance;
    public float horizontalDiagonalSightDistance;
    public Player player;

    private Vector2 up;
    private Vector2 down;
    private Vector2 right;
    private Vector2 left;
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
        up = new Vector2(0.5F, 0.25F);
        down = new Vector2(-0.5F, -0.25F);
        right = new Vector2(0.5F, -0.25F);
        left = new Vector2(-0.5F, 0.25F);
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
                               gameObject.transform.position,
                               direction,
                               ref travelMoveCount,
                               ref targetTilePosition,
                               gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
    }

    private void EnemyBehaviour()
    {
        if (!isMoving && !isTravelling)
        {
            if (EntityUtilities.SearchForPlayer(up,
                                                sightDistance,
                                                ref raycastResultCount,
                                                gameObject.transform.position,
                                                filter,
                                                ref raycastResults))
            {
                travelMoveCount = (int)((player.tilePosition.x - gameObject.transform.position.x) / up.x);
                direction = up;
                isTravelling = true;
            }
            else if (EntityUtilities.SearchForPlayer(down,
                                                     sightDistance,
                                                     ref raycastResultCount,
                                                     gameObject.transform.position,
                                                     filter,
                                                     ref raycastResults))
            {
                travelMoveCount = (int)((player.tilePosition.x - gameObject.transform.position.x) / down.x);
                direction = down;
                isTravelling = true;
            }
            else if (EntityUtilities.SearchForPlayer(right,
                                                     sightDistance,
                                                     ref raycastResultCount,
                                                     gameObject.transform.position,
                                                     filter,
                                                     ref raycastResults))
            {
                travelMoveCount = (int)((player.tilePosition.x - gameObject.transform.position.x) / right.x);
                direction = right;
                isTravelling = true;
            }
            else if (EntityUtilities.SearchForPlayer(left,
                                                     sightDistance,
                                                     ref raycastResultCount,
                                                     gameObject.transform.position,
                                                     filter,
                                                     ref raycastResults))
            {
                travelMoveCount = (int)((player.tilePosition.x - gameObject.transform.position.x) / left.x);
                direction = left;
                isTravelling = true;
            }
            else if (EntityUtilities.SearchForPlayer(upDiagonal,
                                                     verticalDiagonalSightDistance,
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
                                                     verticalDiagonalSightDistance,
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
                                                     horizontalDiagonalSightDistance,
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
                                                     horizontalDiagonalSightDistance,
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
