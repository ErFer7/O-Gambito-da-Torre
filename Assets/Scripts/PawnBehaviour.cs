using System.Collections.Generic;
using UnityEngine;

public class PawnBehaviour : MonoBehaviour
{
    public float smoothTime;
    public float convergenceThreshold;
    public float sightDistance;
    public float horizontalDiagonalSightDistance;
    public float verticalDiagonalSightDistance;
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
            if (EntityUtilities.SearchForPlayer(mainDirection,
                                                sightDistance,
                                                ref raycastResultCount,
                                                gameObject.transform.position,
                                                filter,
                                                ref raycastResults))
            {
                travelMoveCount = ((int)((player.currentTilePosition.x - gameObject.transform.position.x) / mainDirection.x)) - 1;
                direction = mainDirection;
                isTravelling = true;
            }
            else if (EntityUtilities.SearchForPlayer(rightDiagonal,
                                                     verticalDiagonalSightDistance,
                                                     ref raycastResultCount,
                                                     gameObject.transform.position,
                                                     filter,
                                                     ref raycastResults))
            {
                switch (searchDirection)
                {
                    case Direction.Up:

                        travelMoveCount = (int)((player.currentTilePosition.x - gameObject.transform.position.x) / rightDiagonal.x);
                        break;
                    case Direction.Down:

                        travelMoveCount = (int)((player.currentTilePosition.x - gameObject.transform.position.x) / rightDiagonal.x);
                        break;
                    case Direction.Right:

                        travelMoveCount = (int)((player.currentTilePosition.y - gameObject.transform.position.y) / rightDiagonal.y);
                        break;
                    case Direction.Left:

                        travelMoveCount = (int)((player.currentTilePosition.y - gameObject.transform.position.y) / rightDiagonal.y);
                        break;
                    default:
                        break;
                }

                direction = rightDiagonal;
                isTravelling = true;
            }
            else if (EntityUtilities.SearchForPlayer(leftDiagonal,
                                                     verticalDiagonalSightDistance,
                                                     ref raycastResultCount,
                                                     gameObject.transform.position,
                                                     filter,
                                                     ref raycastResults))
            {
                switch (searchDirection)
                {
                    case Direction.Up:

                        travelMoveCount = (int)((player.currentTilePosition.y - gameObject.transform.position.y) / leftDiagonal.y);
                        break;
                    case Direction.Down:

                        travelMoveCount = (int)((player.currentTilePosition.y - gameObject.transform.position.y) / leftDiagonal.y);
                        break;
                    case Direction.Right:

                        travelMoveCount = (int)((player.currentTilePosition.y - gameObject.transform.position.y) / leftDiagonal.y);
                        break;
                    case Direction.Left:

                        travelMoveCount = (int)((player.currentTilePosition.x - gameObject.transform.position.x) / leftDiagonal.x);
                        break;
                    default:
                        break;
                }

                direction = leftDiagonal;
                isTravelling = true;
            }
        }
    }
}
