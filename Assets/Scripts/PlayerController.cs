using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed, gravityModifier, jumpPower;
    public CharacterController charCon;

    private Vector3 moveInput;

    public Transform camTrans;

    //mouse
    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;

    private bool canJump, canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        charCon = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        //store y velocity
        float yStore = moveInput.y;

        float vertMove = Input.GetAxis("Vertical");
        float horiMove = Input.GetAxis("Horizontal");

        Vector3 vert = transform.forward * vertMove;
        Vector3 hori = transform.right * horiMove;

        moveInput = vert + hori;
        moveInput = moveInput * moveSpeed;

        moveInput.y = yStore;

        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

        if(charCon.isGrounded)
        {
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0;

        /*if(canJump)
        {
            canDoubleJump = false;
        }*/

        //Handle jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(canJump)
            {
                moveInput.y = jumpPower;
                canDoubleJump = true;
            }

            else if (canDoubleJump == true)
            {
                moveInput.y = jumpPower;
                canDoubleJump = false;
            }
        }

        charCon.Move(moveInput * moveSpeed * Time.deltaTime);

        // control camera rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        if (invertX)
        {
            mouseInput.x = -mouseInput.x;
        }
        if (invertY)
        {
            mouseInput.y = -mouseInput.y;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));
    }
}
