using UnityEngine;

public class FPSControler : MonoBehaviour
{
    #region Public Variables
    public float velocity;
    public float jumpForce;
    #endregion

    #region Private Variables
    private bool fpsPerspective;
    private Vector3 angles;
    private Transform fpsCamera;

    private Vector3 direction;
    private Vector3 lateralDirection;
    private bool movingDirectionally;
    private bool movingLaterally;
    private bool jumping;
    private bool jumpingAllowed;
    #endregion

    #region Unity Methods
    private void Start()
    {
        fpsCamera = transform.GetChild(0).transform;

        fpsPerspective = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        angles = new Vector3(0F, 0F, 0F);
        direction = new Vector3(0F, 0F, 0F);

        movingDirectionally = false;

        jumping = false;
        jumpingAllowed = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (fpsPerspective)
            {
                fpsPerspective = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                fpsPerspective = true;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }
        }

        if (fpsPerspective)
        {
            angles.x = (-ScreenToCameraPostion(Input.mousePosition).y / (Screen.height / 2F)) * 90F;

            angles.y = ScreenToCameraPostion(Input.mousePosition).x;

            fpsCamera.localEulerAngles = angles;
        }

        ControlInput();
    }

    private void FixedUpdate()
    {
        Control();
    }

    private void OnCollisionStay(Collision collision)
    {
        jumpingAllowed = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        jumpingAllowed = false;
    }
    #endregion

    #region Methods
    Vector3 ScreenToCameraPostion(Vector3 vector)
    {
        vector.x -= Screen.width / 2F;
        vector.y -= Screen.height / 2F;
        vector.z = 0F;

        return vector;
    }

    Vector3 SwapComponentsXZ(Vector3 vector, bool negate, int component = 0)
    {
        float temp = vector.x;
        if (negate && component == 0)
        {
            vector.x = -vector.z;
        }
        else
        {
            vector.x = vector.z;
        }

        if (negate && component == 1)
        {
            vector.z = -temp;
        }
        else
        {
            vector.z = temp;
        }

        return vector;
    }

    void ControlInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            movingDirectionally = true;
            direction = fpsCamera.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movingDirectionally = true;
            direction = -fpsCamera.forward;
        }
        else
        {
            movingDirectionally = false;
            direction.x = 0F;
            direction.z = 0F;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movingLaterally = true;
            lateralDirection = SwapComponentsXZ(fpsCamera.forward, true, 1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movingLaterally = true;
            lateralDirection = SwapComponentsXZ(fpsCamera.forward, true);
        }
        else
        {
            movingLaterally = false;
            lateralDirection.x = 0F;
            lateralDirection.z = 0F;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            jumping = true;
        }
        else
        {
            jumping = false;
        }
    }

    void Control()
    {
        if (movingDirectionally || movingLaterally)
        {
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3((direction.x + lateralDirection.x) * velocity, gameObject.GetComponent<Rigidbody>().velocity.y, (direction.z + lateralDirection.z) * velocity);
        }

        if (jumping && jumpingAllowed)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    #endregion
}