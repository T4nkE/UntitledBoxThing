using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed;//Speed
    private float move;

    public float jump; //Jump Power

    public float deathCounter; //Counts up the deaths on a level, currently useless

    public bool isJumping; //Jump Check asset

    public float TorqueSpeed; //Speed of the spin

    const float angularVelocityThreshold = 1e-5f;

    private const float inputCooldown = 1.5f; // Time between input detection in seconds
    private float inputCooldownTimer = 0f;

    private float exponentialTorque = 0f; // Exponential torque variable
    public float torqueMultiplier = 1.0000001f; // Exponential increase

    private Rigidbody2D rb;

    public Text DeathCounterText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        move = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(speed * move, rb.velocity.y);

        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isJumping)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump));
        }

    //Angles
        // Check input cooldown
        inputCooldownTimer -= Time.deltaTime;

        // Adding exponential angular speed
        if (inputCooldownTimer <= 0f)
        {
            if (Input.GetKey(KeyCode.Q) && isJumping)
            {
                exponentialTorque = TorqueSpeed * torqueMultiplier;
                rb.AddTorque(exponentialTorque);
                inputCooldownTimer = inputCooldown;
            }
            if (Input.GetKey(KeyCode.E) && isJumping)
            {
                exponentialTorque = TorqueSpeed * torqueMultiplier;
                rb.AddTorque(-exponentialTorque);
                inputCooldownTimer = inputCooldown;
            }
        }

        //Removing angular speed
        if (Input.GetKeyUp(KeyCode.Q) && isJumping)
        {
            if (TorqueSpeed > 0)
            {
                rb.AddTorque(-TorqueSpeed * 0.5f);
            }
        }
        if (Input.GetKeyUp(KeyCode.E) && isJumping)
        {
            if (TorqueSpeed < 0)
            {
                rb.AddTorque(TorqueSpeed * 0.5f);
            }
        }
        //Other angle stuff
        if (Mathf.Abs(rb.angularVelocity) <= angularVelocityThreshold)
        {
            rb.angularVelocity = 0;
        }
    }

//Jump Checks

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }

        if (other.gameObject.CompareTag("Death"))
        {
            deathCounter += 1;
            UpdateDeathCounterDisplay();
            rb.velocity = Vector2.zero; // Reset velocity to zero on death
            rb.angularVelocity = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
        }
    }

    private void UpdateDeathCounterDisplay()
    {
        DeathCounterText.text = "Deaths: " + deathCounter.ToString();
    }
}
