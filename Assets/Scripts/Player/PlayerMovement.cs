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
    }

    public Vector2 GetFriction()
    {
        return moveDirection != Vector2.zero ? moveFriction : stopFriction;
    }

    public void MoveBound()
    {

    }

    public bool IsMoving()
    {
        return moveVelocity != Vector2.zero;
    }
}
