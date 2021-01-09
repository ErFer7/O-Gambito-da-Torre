using System.Collections.Generic;
using UnityEngine;

public class PawnBehaviour : MonoBehaviour
{
    public float smoothTime;
    public float convergenceThreshold;
    public float sightDistance;
    public float verticalDiagonalSightDistance;
    public float horizontalDiagonalSightDistance;
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
    private float rightDiagonalSightDistance;
    private float leftDiagonalSightDistance;
    private int travelMoveCount;

    private void Start()
    {
        switch (searchDirection)
        {
            case Direction.Up:

                mainDirection = new Vector2(0.5F, 0.25F);
                rightDiagonal = new Vector2(1.0F, 0.0F);
                leftDiagonal = new Vector2(0.0F, 0.5F);

                rightDiagonalSightDistance = horizontalDiagonalSightDistance;
                leftDiagonalSightDistance = verticalDiagonalSightDistance;
                break;
            case Direction.Down:

                mainDirection = new Vector2(-0.5F, -0.25F);
                rightDiagonal = new Vector2(-1.0F, 0.0F);
                leftDiagonal = new Vector2(0.0F, -0.5F);

                rightDiagonalSightDistance = horizontalDiagonalSightDistance;
                leftDiagonalSightDistance = verticalDiagonalSightDistance;
                break;
            case Direction.Right:

                mainDirection = new Vector2(0.5F, -0.25F);
                rightDiagonal = new Vector2(0.0F, -0.5F);
                leftDiagonal = new Vector2(1.0F, 0.0F);

                rightDiagonalSightDistance = verticalDiagonalSightDistance;
                leftDiagonalSightDistance = horizontalDiagonalSightDistance;
                break;
            case Direction.Left:

                mainDirection = new Vector2(-0.5F, 0.25F);
                rightDiagonal = new Vector2(0.0F, 0.5F);
                leftDiagonal = new Vector2(-1.0F, 0.0F);

                rightDiagonalSightDistance = verticalDiagonalSightDistance;
                leftDiagonalSightDistance = horizontalDiagonalSightDistance;
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
                               gameObject.transform.position,
                               direction,
                               ref travelMoveCount,
                               ref targetTilePosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (isMoving && targetTilePosition == player.tilePosition)
            {

                Destroy(collision.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
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
                                                ref raycastResults,
                                                gameObject))
            {
                if (player.tilePosition.x != gameObject.transform.position.x &&
                    player.tilePosition.y != gameObject.transform.position.y)
                {
                    travelMoveCount = ((int)((player.tilePosition.x - gameObject.transform.position.x) /
                                            mainDirection.x)) - 1;
                    direction = mainDirection;
                    isTravelling = true;
                }
            }
            else if (EntityUtilities.SearchForPlayer(rightDiagonal,
                                                     rightDiagonalSightDistance,
                                                     ref raycastResultCount,
                                                     gameObject.transform.position,
                                                     filter,
                                                     ref raycastResults,
                                                     gameObject))
            {
                travelMoveCount = 1;
                direction = rightDiagonal;
                isTravelling = true;
            }
            else if (EntityUtilities.SearchForPlayer(leftDiagonal,
                                                     leftDiagonalSightDistance,
                                                     ref raycastResultCount,
                                                     gameObject.transform.position,
                                                     filter,
                                                     ref raycastResults,
                                                     gameObject))
            {
                travelMoveCount = 1;
                direction = leftDiagonal;
                isTravelling = true;
            }
        }
    }
}
