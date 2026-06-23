using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    
    private Rigidbody2D rb;
    private Vector2 input;

    private bool isDashing;
    private float horizontalMovement;
    private float verticalMovement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;

        rb.linearVelocity = input * moveSpeed;
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        rb.linearVelocity = dashSpeed * input;
        
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }
    
    public void GetMovementInput(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        verticalMovement = context.ReadValue<Vector2>().y;
        input = new Vector2(horizontalMovement, verticalMovement);
        input.Normalize();
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        LookAt(mousePosition);
    }

    protected void LookAt(Vector3 target)
    {
        float lookAngle = AngleBetweenTwoPoints(transform.position, target) + 180;
        
        transform.eulerAngles = new Vector3(0, 0, lookAngle);
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
