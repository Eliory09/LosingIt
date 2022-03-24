using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D physics;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 initialPosition;
    private bool _areArrowsPressed;
    private bool resetAllowed;

    public int counter;

    private void Awake()
    {
        resetAllowed = false;
        StartCoroutine(ResetCooldown(1f));
    }

    private void Update()
    {
        if (Camera.main is { })
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
            bool isVisible = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 &&
                             screenPoint.y < 1;
            if ((isVisible && counter != 0) || !resetAllowed) return;
            resetAllowed = false;
            StartCoroutine(ResetCooldown(1));
            GameManager.ActivateRoundLoss();

        }
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
        // if (Input.GetKey(KeyCode.UpArrow))
        // {
        //     physics.AddForce(Vector2.up * speed);
        //     _areArrowsPressed = true;
        // }
        //
        // if (Input.GetKey(KeyCode.DownArrow))
        // {
        //     physics.AddForce(Vector2.down * speed);
        //     _areArrowsPressed = true;
        // }
        //
        // if (Input.GetKey(KeyCode.LeftArrow))
        // {
        //     physics.AddForce(Vector2.left * speed);
        //     _areArrowsPressed = true;
        // }
        //
        // if (Input.GetKey(KeyCode.RightArrow))
        // {
        //     physics.AddForce(Vector2.right * speed);
        //     _areArrowsPressed = true;
        // }
        //
        // if (_areArrowsPressed)
        // {
        //     _areArrowsPressed = false;
        // }
        // else
        // {
        //     physics.AddForce(-physics.velocity.normalized * 3 * speed);
        // }
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

    public void ResetBall()
    {
        var trail = GetComponent<TrailRenderer>();
        transform.position = initialPosition;
        physics.velocity = Vector2.zero;
        trail.Clear();
    }

    private IEnumerator ResetCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        resetAllowed = true;
    }
}