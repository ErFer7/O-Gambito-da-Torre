using UnityEngine;

public class KnightBehaviour : MonoBehaviour
{
    public float smoothTime;
    public float convergenceThreshold;
    public float detectionThreshold;
    public Player player;

    private Vector2 up;
    private Vector2 down;
    private Vector2 right;
    private Vector2 left;
    private Vector2 upRightPosition;
    private Vector2 upLeftPosition;
    private Vector2 downRightPosition;
    private Vector2 downLeftPosition;
    private Vector2 rightUpPosition;
    private Vector2 rightDownPosition;
    private Vector2 leftUpPosition;
    private Vector2 leftDownPosition;
    private Vector2 direction;
    private Vector2 pivotDirection;
    private Vector2 targetTilePosition;
    private Vector2 velocity;
    private bool isMoving;
    private bool isTravelling;
    private int travelMoveCount;

    private void Start()
    {
        up = new Vector2(0.5F, 0.25F);
        down = new Vector2(-0.5F, -0.25F);
        right = new Vector2(0.5F, -0.25F);
        left = new Vector2(-0.5F, 0.25F);
        upRightPosition = new Vector2(1.5F, 0.25F);
        upLeftPosition = new Vector2(0.5F, 0.75F);
        downRightPosition = new Vector2(-0.5F, -0.75F);
        downLeftPosition = new Vector2(-1.5F, -0.25F);
        rightUpPosition = new Vector2(1.5F, -0.25F);
        rightDownPosition = new Vector2(0.5F, -0.75F);
        leftUpPosition = new Vector2(-0.5F, 0.75F);
        leftDownPosition = new Vector2(-1.5F, 0.25F);
        velocity = Vector2.zero;

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

        if (travelMoveCount == 1)
        {
            direction = pivotDirection;
        }

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
            if (Vector2.Distance(player.tilePosition, (Vector2)gameObject.transform.position + upRightPosition) < detectionThreshold)
            {
                travelMoveCount = 2;
                direction = up * 2;
                pivotDirection = right;
                isTravelling = true;
            }
            else if (Vector2.Distance(player.tilePosition, (Vector2)gameObject.transform.position + upLeftPosition) < detectionThreshold)
            {
                travelMoveCount = 2;
                direction = up * 2;
                pivotDirection = left;
                isTravelling = true;
            }
            else if (Vector2.Distance(player.tilePosition, (Vector2)gameObject.transform.position + downRightPosition) < detectionThreshold)
            {
                travelMoveCount = 2;
                direction = down * 2;
                pivotDirection = right;
                isTravelling = true;
            }
            else if (Vector2.Distance(player.tilePosition, (Vector2)gameObject.transform.position + downLeftPosition) < detectionThreshold)
            {
                travelMoveCount = 2;
                direction = down * 2;
                pivotDirection = left;
                isTravelling = true;
            }
            else if (Vector2.Distance(player.tilePosition, (Vector2)gameObject.transform.position + rightUpPosition) < detectionThreshold)
            {
                travelMoveCount = 2;
                direction = right * 2;
                pivotDirection = up;
                isTravelling = true;
            }
            else if (Vector2.Distance(player.tilePosition, (Vector2)gameObject.transform.position + rightDownPosition) < detectionThreshold)
            {
                travelMoveCount = 2;
                direction = right * 2;
                pivotDirection = down;
                isTravelling = true;
            }
            else if (Vector2.Distance(player.tilePosition, (Vector2)gameObject.transform.position + leftUpPosition) < detectionThreshold)
            {
                travelMoveCount = 2;
                direction = left * 2;
                pivotDirection = up;
                isTravelling = true;
            }
            else if (Vector2.Distance(player.tilePosition, (Vector2)gameObject.transform.position + leftDownPosition) < detectionThreshold)
            {
                travelMoveCount = 2;
                direction = left * 2;
                pivotDirection = down;
                isTravelling = true;
            }
        }
    }
}
