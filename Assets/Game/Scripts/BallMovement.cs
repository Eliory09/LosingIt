using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D physics;
    [SerializeField] private float speed;
    private bool _areArrowsPressed;

    public int counter;

    private void Update()
    {
        if (counter == 0)
        {
            print("dead");
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
}