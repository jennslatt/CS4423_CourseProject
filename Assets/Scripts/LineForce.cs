using System;
using UnityEngine;

public class LineForce : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float stopVelocity = .05f;
    [SerializeField] private float shotPower = 150f;
    private bool isIdle;
    private bool isAiming;
    private bool gameWon = false;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody playerRigidbody;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        isAiming = false;
        lineRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        if (playerRigidbody.linearVelocity.magnitude < stopVelocity)
        {
            Stop();
        }

        ProcessAim();

        if (!gameWon)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            if (Input.GetKey(KeyCode.S)) 
            {
                playerRigidbody.AddForce(Vector3.forward * moveSpeed, ForceMode.Force);
            }


            if (Input.GetKey(KeyCode.W))
            {
                playerRigidbody.AddForce(Vector3.back * moveSpeed, ForceMode.Force);
            }

            if (Input.GetKey(KeyCode.D))
            {
                playerRigidbody.AddForce(Vector3.left * moveSpeed, ForceMode.Force);
            }

            if (Input.GetKey(KeyCode.A))
            {
                playerRigidbody.AddForce(Vector3.right * moveSpeed, ForceMode.Force);

            }
        }
    }

    private void OnMouseDown()
    {
        if (isIdle)
        {
            isAiming = true;
        }
    }

    private void ProcessAim()
    {
        if (!isAiming || !isAiming)
        {
            return;
        }

        Vector3? worldPoint = CastMouseClickRay();

        if (!worldPoint.HasValue)
        {
            return;
        }
        DrawLine(worldPoint.Value);

        if (Input.GetMouseButtonUp(0))
        {
            Shoot(worldPoint.Value);
        }
    }

    private void Shoot(Vector3 worldPoint)
    {
        isAiming = false;
        lineRenderer.enabled = false;
        Vector3 horizontalWorldPoint = new Vector3(worldPoint.x, transform.position.y, worldPoint.y);

        Vector3 direction = (horizontalWorldPoint - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, horizontalWorldPoint);
        playerRigidbody.AddForce(direction * strength * shotPower);
        isIdle = false;
    }
   

    
    private void DrawLine(Vector3 worldPoint)
    {
        Vector3[] positions = { transform.position, worldPoint };
        lineRenderer.SetPositions(positions);
        lineRenderer.enabled = true;

    }

    private void Stop()
    {
        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
        isIdle = true;
    }

    private void OnTriggerEnter(Collider other){

        gameWon = true;
        isAiming = false;
        lineRenderer.enabled = false;
        
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            playerRigidbody.isKinematic = true;
        }

        Debug.Log("You Win!");
    }


    private Vector3? CastMouseClickRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        if (Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, float.PositiveInfinity))
        {
            return hit.point;
        }
        else
        {
            return null;
        }
    }
}
