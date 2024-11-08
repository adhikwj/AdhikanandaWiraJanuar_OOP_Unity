using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 maxSpeed;
    [SerializeField] private Vector2 timeToFullSpeed;
    [SerializeField] private Vector2 timeToStop;
    [SerializeField] private Vector2 stopClamp;
    private Vector2 moveDirection;
    private Vector2 moveVelocity;
    private Vector2 moveFriction;
    private Vector2 stopFriction;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveVelocity = 2 * maxSpeed / timeToFullSpeed;
        moveFriction = -2 * maxSpeed / (timeToFullSpeed * timeToFullSpeed);
        stopFriction = -2 * maxSpeed / (timeToStop * timeToStop);
    }

    public void Move()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), 
        Input.GetAxis("Vertical"));

        if (moveDirection != Vector2.zero)
        {
            Vector2 targetVelocity = moveDirection.normalized * maxSpeed;

            moveVelocity = Vector2.MoveTowards(moveVelocity, targetVelocity, 
            moveFriction.magnitude * Time.deltaTime);

            moveVelocity = new Vector2(
                Mathf.Clamp(moveVelocity.x, -maxSpeed.x, maxSpeed.x),
                Mathf.Clamp(moveVelocity.y, -maxSpeed.y, maxSpeed.y)
            );
        }
        else
        {
            moveVelocity = Vector2.MoveTowards(moveVelocity, Vector2.zero, 
            stopFriction.magnitude * Time.deltaTime);

            if (moveVelocity.magnitude < stopClamp.magnitude)
            {
                moveVelocity = Vector2.zero;
            }
        }

        rb.velocity = moveVelocity;
        MoveBound();
    }

    public Vector2 GetFriction()
    {
        return moveDirection != Vector2.zero ? moveFriction : stopFriction;
    }

    public void MoveBound()
    {
        // Get the camera instance
        Camera mainCamera = Camera.main;

        // Convert the ship's position to viewport coordinates (values between 0 and 1)
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        // Clamp the viewport position so the ship stays within bounds (0 to 1 for x and y)
        viewportPos.x = Mathf.Clamp(viewportPos.x, 0.025f, 0.975f);
        viewportPos.y = Mathf.Clamp(viewportPos.y, -0.01f, 0.88f);

        // Convert back to world position
        transform.position = mainCamera.ViewportToWorldPoint(viewportPos);
    }

    public bool IsMoving()
    {
        return moveVelocity != Vector2.zero;
    }
}
