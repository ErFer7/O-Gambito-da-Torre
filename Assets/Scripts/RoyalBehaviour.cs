using System.Collections.Generic;
using UnityEngine;

public class RoyalBehaviour : MonoBehaviour
{
    public float smoothTime;
    public float convergenceThreshold;
    public float sightDistance;
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
        Control();
    }

    private void Update()
    {
        EnemyBehaviour();
        Travel();
    }

    private void EnemyBehaviour()
    {
        if (!isMoving && !isTravelling)
        {
            if (SearchForPlayer(up, sightDistance))
            {
                travelMoveCount = (int)((player.currentTilePosition.x - gameObject.transform.position.x) / up.x);
                direction = up;
                isTravelling = true;
            }
            else if (SearchForPlayer(down, sightDistance))
            {
                travelMoveCount = (int)((player.currentTilePosition.x - gameObject.transform.position.x) / down.x);
                direction = down;
                isTravelling = true;
            }
            else if (SearchForPlayer(right, sightDistance))
            {
                travelMoveCount = (int)((player.currentTilePosition.x - gameObject.transform.position.x) / right.x);
                direction = right;
                isTravelling = true;
            }
            else if (SearchForPlayer(left, sightDistance))
            {
                travelMoveCount = (int)((player.currentTilePosition.x - gameObject.transform.position.x) / left.x);
                direction = left;
                isTravelling = true;
            }
            else if (SearchForPlayer(upDiagonal, sightDistance))
            {
                travelMoveCount = (int)((player.currentTilePosition.y - gameObject.transform.position.y) / upDiagonal.y);
                direction = upDiagonal;
                isTravelling = true;
            }
            else if (SearchForPlayer(downDiagonal, sightDistance))
            {
                travelMoveCount = (int)((player.currentTilePosition.y - gameObject.transform.position.y) / downDiagonal.y);
                direction = downDiagonal;
                isTravelling = true;
            }
            else if (SearchForPlayer(rightDiagonal, sightDistance))
            {
                travelMoveCount = (int)((player.currentTilePosition.x - gameObject.transform.position.x) / rightDiagonal.x);
                direction = rightDiagonal;
                isTravelling = true;
            }
            else if (SearchForPlayer(leftDiagonal, sightDistance))
            {
                travelMoveCount = (int)((player.currentTilePosition.x - gameObject.transform.position.x) / leftDiagonal.x);
                direction = leftDiagonal;
                isTravelling = true;
            }
        }
    }

    private bool SearchForPlayer(Vector2 direction, float distance)
    {
        bool playerFound = false;

        raycastResultCount = Physics2D.Raycast(gameObject.transform.position,
                                                         direction,
                                                         filter.NoFilter(),
                                                         raycastResults,
                                                         distance);
        for (int i = 0; i < raycastResultCount; i++)
        {
            if (raycastResults[i].collider.tag == "Player")
            {
                playerFound = true;
                break;
            }
            else if (raycastResults[i].collider.tag == "Scenery")
            {
                break;
            }
        }

        return playerFound;
    }

    private void Travel()
    {
        if (isTravelling)
        {
            if (travelMoveCount > 0)
            {
                targetTilePosition = (Vector2)gameObject.transform.position + direction;
                isMoving = true;
                travelMoveCount--;
            }

            isTravelling = false;
        }
    }

    private void Control()
    {
        if (isMoving)
        {
            gameObject.transform.position = Vector2.SmoothDamp(gameObject.transform.position, targetTilePosition, ref velocity, smoothTime);

            if (Vector2.Distance(gameObject.transform.position, targetTilePosition) < convergenceThreshold)
            {
                gameObject.transform.position = targetTilePosition;
                gameObject.transform.position = EntityUtilities.AlignPosition(gameObject.transform.position);
                isTravelling = true;

                isMoving = false;
            }
        }
    }
}
