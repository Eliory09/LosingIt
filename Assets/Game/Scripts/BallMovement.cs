using System.Collections;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    #region Fields

    [SerializeField] private Rigidbody2D physics;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 initialPosition;
    private bool _areArrowsPressed;

    public int counter;

    #endregion

    #region MonoBehaviour

    private void Update()
    {
        if (Camera.main is null) return;
        var screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        var isVisible = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 &&
                        screenPoint.y < 1;
        if (isVisible && counter != 0 || !GameManager.IsResetAllowed()) return;
        GameManager.ActivateRoundLoss();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("InitialPlatform"))
        {
            counter++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("InitialPlatform"))
        {
            counter--;
        }
    }


    void FixedUpdate()
    {
        Vector2 currentVel = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentVel += Vector2.up;
            _areArrowsPressed = true;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            currentVel += Vector2.down;
            _areArrowsPressed = true;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentVel += Vector2.left;
            _areArrowsPressed = true;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentVel += Vector2.right;
            _areArrowsPressed = true;
        }

        if (_areArrowsPressed)
        {
            _areArrowsPressed = false;
            physics.velocity = currentVel * speed;
        }
        else
        {
            if (physics.velocity.x < 0.05f || physics.velocity.y < 0.05f)
                physics.velocity = Vector2.zero;
            else if (physics.velocity != Vector2.zero)
                physics.velocity -= physics.velocity.normalized * 7 * speed * Time.deltaTime;
        }
    }

    #endregion


    #region Methods

    public void ResetBall()
    {
        var trail = GetComponent<TrailRenderer>();
        transform.position = initialPosition;
        physics.velocity = Vector2.zero;
        trail.Clear();
    }

    #endregion
}