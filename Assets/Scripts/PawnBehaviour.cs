using System.Collections.Generic;
using UnityEngine;

public class PawnBehaviour : MonoBehaviour
{
    public float smoothTime;
    public float convergenceThreshold;
    public float sightDistance;
    public Direction searchDirection;
    public Player player;

    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }

    private Vector2 mainDirection;
    private Vector2 rightDiagonal;
    private Vector2 leftDiagonal;
    private Vector2 targetTilePosition;
    private Vector2 velocity;
    private Vector2 direction;
    private ContactFilter2D filter;
    private int raycastResultCount;
    private List<RaycastHit2D> raycastResults;
    private bool isMoving;
    private bool isTravelling;
    private int travelMoveCount;

    private void Start()
    {
        switch (searchDirection)
        {
            case Direction.Up:

                mainDirection = new Vector2(0.5F, 0.25F);
                rightDiagonal = new Vector2(1.0F, 0.0F);
                leftDiagonal = new Vector2(0.0F, 0.5F);
                break;
            case Direction.Down:

                mainDirection = new Vector2(-0.5F, -0.25F);
                rightDiagonal = new Vector2(-1.0F, 0.0F);
                leftDiagonal = new Vector2(0.0F, -0.5F);
                break;
            case Direction.Right:

                mainDirection = new Vector2(0.5F, -0.25F);
                rightDiagonal = new Vector2(0.0F, -0.5F);
                leftDiagonal = new Vector2(1.0F, 0.0F);
                break;
            case Direction.Left:

                mainDirection = new Vector2(-0.5F, 0.25F);
                rightDiagonal = new Vector2(0.0F, 0.5F);
                leftDiagonal = new Vector2(-1.0F, 0.0F);
                break;
            default:
                break;
        }

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
            if (SearchForPlayer(mainDirection, sightDistance))
            {
                travelMoveCount = ((int)((player.currentTilePosition.x - gameObject.transform.position.x) / mainDirection.x)) - 1;
                direction = mainDirection;
                isTravelling = true;
            }
            else if (SearchForPlayer(rightDiagonal, sightDistance))
            {
                // Arrumar a lógica com várias diagonais
                travelMoveCount = (int)((player.currentTilePosition.x - gameObject.transform.position.x) / mainDirection.x);
                direction = rightDiagonal;
                isTravelling = true;
            }
            else if (SearchForPlayer(leftDiagonal, sightDistance))
            {
                travelMoveCount = (int)((player.currentTilePosition.x - gameObject.transform.position.x) / mainDirection.x);
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
