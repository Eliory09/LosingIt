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
        if (other.gameObject.CompareTag("Block"))
        {
            counter++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Block"))
        {
            counter--;
        }
    }


    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            physics.AddForce(Vector2.up * speed);
            _areArrowsPressed = true;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            physics.AddForce(Vector2.down * speed);
            _areArrowsPressed = true;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            physics.AddForce(Vector2.left * speed);
            _areArrowsPressed = true;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            physics.AddForce(Vector2.right * speed);
            _areArrowsPressed = true;
        }

        if (_areArrowsPressed)
        {
            _areArrowsPressed = false;
        }
        else
        {
            physics.AddForce(-physics.velocity.normalized * 3 * speed);
        }
    }

    public void ResetBall()
    {
        transform.position = initialPosition;
        physics.velocity = Vector2.zero;
    }

    private IEnumerator ResetCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        resetAllowed = true;
    }
}