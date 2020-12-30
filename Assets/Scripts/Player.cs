﻿using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Public Variables
    public float smoothTime;
    public float convergenceThreshold;

    [System.NonSerialized]
    public Vector2 currentTilePosition;
    #endregion

    #region Private Variables
    private Vector2 up;
    private Vector2 down;
    private Vector2 right;
    private Vector2 left;
    private Vector2 targetTilePosition;
    private Vector2 velocity;
    private ContactFilter2D filter;
    private int raycastResultCount;
    private List<RaycastHit2D> raycastResults;
    private bool isMoving;
    #endregion

    #region Unity Methods
    void Start()
    {
        currentTilePosition = gameObject.transform.position;

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
                    targetTilePosition = (Vector2)gameObject.transform.position + up;
                    currentTilePosition = targetTilePosition;
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                raycastResultCount = Physics2D.Raycast(gameObject.transform.position, down, filter.NoFilter(), raycastResults, 0.5F);

                if (raycastResultCount <= 1)
                {
                    isMoving = true;
                    targetTilePosition = (Vector2)gameObject.transform.position + down;
                    currentTilePosition = targetTilePosition;
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                raycastResultCount = Physics2D.Raycast(gameObject.transform.position, right, filter.NoFilter(), raycastResults, 0.5F);

                if (raycastResultCount <= 1)
                {
                    isMoving = true;
                    targetTilePosition = (Vector2)gameObject.transform.position + right;
                    currentTilePosition = targetTilePosition;
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                raycastResultCount = Physics2D.Raycast(gameObject.transform.position, left, filter.NoFilter(), raycastResults, 0.5F);

                if (raycastResultCount <= 1)
                {
                    isMoving = true;
                    targetTilePosition = (Vector2)gameObject.transform.position + left;
                    currentTilePosition = targetTilePosition;
                }
            }
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

                isMoving = false;
            }
        }
    }
    #endregion
}