using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 10f;

    private Animator anim;
    private Rigidbody rb;
    private Transform cam;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    void FixedUpdate()
    { 
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0, v).normalized;

        if (input.magnitude >= 0.1f)
        {
            Vector3 camForward = cam.forward;
            camForward.y = 0;

            Vector3 moveDir = (camForward * v + cam.right * h).normalized;

            // PHYSICS MOVEMENT
            Vector3 targetPosition = rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);

            // ROTATION
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * turnSpeed));

            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }
}