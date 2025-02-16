using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.InputSystem.InputAction;

public class Controller : MonoBehaviour
{
    [SerializeField]private bool isOnGround = false, jumpRequest = false;
    [SerializeField] private GameObject GameOverCanvas;
    public LayerMask groundLayer;
    private float moveSpeed = 15f, jumpForce = 5f;

    [SerializeField]private Rigidbody rb;
    Vector3 moveVector = Vector3.zero;

    [SerializeField]private InputSystem_Actions playerInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveVector * Time.deltaTime;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        moveVector = new Vector3(direction.x,0,direction.y) * moveSpeed;
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (isOnGround)
        {
            jumpRequest = true;
        }
    }

    private void JumpUpdate()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        if (!isOnGround)
        {
            Vector3 tvelocity = rb.linearVelocity;
            tvelocity.y /= 2.5f;
            rb.linearVelocity = tvelocity;
        }
    }

    void FixedUpdate() {
        isOnGround = Physics.Raycast(transform.position, Vector3.down, 0.5f, groundLayer);
        if (jumpRequest)
        {
            JumpUpdate();
            jumpRequest = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context) {
        //Debug.Log("sono dentro");
        if (context.performed)
        {
            Debug.Log("Interact");
            GameOverCanvas.SetActive(true);
            Time.timeScale = 0;
            StartCoroutine(GameOverReset_EC());
        }
    }


    IEnumerator GameOverReset_EC() {
        Debug.Log("sono dentro");
        yield return new WaitForSecondsRealtime(3);
        Debug.Log("sono fuori!");
        //tolgo il game over
        GameOverCanvas.SetActive(false);
        Time.timeScale = 1;
        //ricarico la scena
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
