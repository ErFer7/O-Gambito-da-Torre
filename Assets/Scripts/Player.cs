using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Public Variables
    public float smoothTime;
    public float convergenceThreshold;
    #endregion

    #region Private Variables
    private Rigidbody2D playerRigidbody;
    private Vector2 up;
    private Vector2 down;
    private Vector2 right;
    private Vector2 left;
    private Vector2 startingPosition;
    private Vector2 targetPosition;
    private Vector2 velocity;
    private ContactFilter2D filter;
    private int raycastResultCount;
    private List<RaycastHit2D> raycastResults;
    private bool isMoving;
    #endregion

    #region Unity Methods
    void Start()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();

        up = new Vector2(0.5F, 0.25F);
        down = new Vector2(-0.5F, -0.25F);
        right = new Vector2(0.5F, -0.25F);
        left = new Vector2(-0.5F, 0.25F);

        velocity = Vector2.zero;

        filter = new ContactFilter2D();

        raycastResults = new List<RaycastHit2D>();

        isMoving = false;
    }

    private void FixedUpdate()
    {
        Control();
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
                raycastResultCount = Physics2D.Raycast(gameObject.transform.position, up, filter.NoFilter(), raycastResults, 0.5F);

                if (raycastResultCount <= 1)
                {
                    isMoving = true;
                    startingPosition = gameObject.transform.position;
                    targetPosition = (Vector2)gameObject.transform.position + up;
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                raycastResultCount = Physics2D.Raycast(gameObject.transform.position, down, filter.NoFilter(), raycastResults, 0.5F);

                if (raycastResultCount <= 1)
                {
                    isMoving = true;
                    startingPosition = gameObject.transform.position;
                    targetPosition = (Vector2)gameObject.transform.position + down;
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                raycastResultCount = Physics2D.Raycast(gameObject.transform.position, right, filter.NoFilter(), raycastResults, 0.5F);

                if (raycastResultCount <= 1)
                {
                    isMoving = true;
                    startingPosition = gameObject.transform.position;
                    targetPosition = (Vector2)gameObject.transform.position + right;
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                raycastResultCount = Physics2D.Raycast(gameObject.transform.position, left, filter.NoFilter(), raycastResults, 0.5F);

                if (raycastResultCount <= 1)
                {
                    isMoving = true;
                    startingPosition = gameObject.transform.position;
                    targetPosition = (Vector2)gameObject.transform.position + left;
                }
            }
        }
    }

    private void Control()
    {
        if (isMoving)
        {
            gameObject.transform.position = Vector2.SmoothDamp(gameObject.transform.position, targetPosition, ref velocity, smoothTime);

            if (Vector2.Distance(gameObject.transform.position, targetPosition) < convergenceThreshold)
            {
                gameObject.transform.position = targetPosition;
                gameObject.transform.position = AlignPosition(gameObject.transform.position);

                isMoving = false;
            }
        }
    }

    // Esta função é possívelmente inútil agora. Mas ela poderá ser usada para evitar que a peça saia do "trilho" de movimento
    private Vector2 AlignPosition(Vector2 vector)
    {
        float xFix = Mathf.Floor(vector.x / 0.5F) * 0.5F - vector.x;
        float yFix = Mathf.Floor(vector.y / 0.25F) * 0.25F - vector.y;

        Vector2 alignedVector = new Vector2(vector.x + xFix, vector.y + yFix);

        return alignedVector;
    }
    #endregion
}
