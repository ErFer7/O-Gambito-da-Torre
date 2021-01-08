using System.Collections.Generic;
using UnityEngine;

public class RookBehaviour : MonoBehaviour
{
    public float smoothTime;
    public float convergenceThreshold;
    public float sightDistance;
    public Player player;

    private Vector2 up;
    private Vector2 down;
    private Vector2 right;
    private Vector2 left;
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
        }
    }
}
