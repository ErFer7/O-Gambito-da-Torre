using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Public Variables
    public float smoothTime;
    public float convergenceThreshold;

    [System.NonSerialized]
    public Vector2 tilePosition;
    #endregion

    #region Private Variables
    private Vector2 up;
    private Vector2 down;
    private Vector2 right;
    private Vector2 left;
    private Vector2 velocity;
    private ContactFilter2D filter;
    private int raycastResultCount;
    private List<RaycastHit2D> raycastResults;
    private bool isMoving;
    private bool canMove;
    #endregion

    #region Unity Methods
    void Start()
    {
        tilePosition = gameObject.transform.position;

        up = new Vector2(0.5F, 0.25F);
        down = new Vector2(-0.5F, -0.25F);
        right = new Vector2(0.5F, -0.25F);
        left = new Vector2(-0.5F, 0.25F);
        velocity = Vector2.zero;

        filter = new ContactFilter2D();

        raycastResults = new List<RaycastHit2D>();

        isMoving = false;
        canMove = false;
    }

    private void FixedUpdate()
    {
        EntityUtilities.PlayerControl(ref isMoving,
                                      gameObject,
                                      ref tilePosition,
                                      ref velocity,
                                      smoothTime,
                                      convergenceThreshold);
    }

    private void Update()
    {
        ControlInput();
    }
    #endregion

    #region Methods
    private void ControlInput()
    {
        if (!isMoving)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                raycastResultCount = Physics2D.Raycast(gameObject.transform.position,
                                                       up,
                                                       filter.NoFilter(),
                                                       raycastResults,
                                                       0.5F);

                if (raycastResultCount <= 1)
                {
                    canMove = true;
                }
                else if (!raycastResults[1].collider.CompareTag("Scenery"))
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }

                if (canMove)
                {
                    isMoving = true;
                    tilePosition = (Vector2)gameObject.transform.position + up;
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                raycastResultCount = Physics2D.Raycast(gameObject.transform.position,
                                                       down,
                                                       filter.NoFilter(),
                                                       raycastResults,
                                                       0.5F);

                if (raycastResultCount <= 1)
                {
                    canMove = true;
                }
                else if (!raycastResults[1].collider.CompareTag("Scenery"))
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }

                if (canMove)
                {
                    isMoving = true;
                    tilePosition = (Vector2)gameObject.transform.position + down;
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                raycastResultCount = Physics2D.Raycast(gameObject.transform.position,
                                                       right,
                                                       filter.NoFilter(),
                                                       raycastResults,
                                                       0.5F);

                if (raycastResultCount <= 1)
                {
                    canMove = true;
                }
                else if (!raycastResults[1].collider.CompareTag("Scenery"))
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }

                if (canMove)
                {
                    isMoving = true;
                    tilePosition = (Vector2)gameObject.transform.position + right;
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                raycastResultCount = Physics2D.Raycast(gameObject.transform.position,
                                                       left,
                                                       filter.NoFilter(),
                                                       raycastResults,
                                                       0.5F);

                if (raycastResultCount <= 1)
                {
                    canMove = true;
                }
                else if (!raycastResults[1].collider.CompareTag("Scenery"))
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }

                if (canMove)
                {
                    isMoving = true;
                    tilePosition = (Vector2)gameObject.transform.position + left;
                }
            }
        }
    }
    #endregion
}
