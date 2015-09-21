using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float moveTime = 0.1f;
    public float inverseMoveTime;

    public LayerMask blockingLayer;

    private Animator animator;
    private BoxCollider2D boxCollider;      //The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;               //The Rigidbody2D component attached to this object.

    private bool isFlipped;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime;
        isFlipped = false;
    }

    // Update is called once per frame
    void Update() {
        int horizontal = 0;     //Used to store the horizontal move direction.
        int vertical = 0;       //Used to store the vertical move direction.

        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if(horizontal != 0 || vertical != 0) {
            if (!isFlipped && horizontal < 0) {
                Vector3 flip = transform.localScale;
                flip.x *= -1;
                transform.localScale = flip;
                transform.position += (Vector3.right * 0.5f);
                isFlipped = true;
            }
            else if (isFlipped && horizontal > 0) {
                Vector3 flip = transform.localScale;
                flip.x = Mathf.Abs(flip.x);
                transform.localScale = flip;
                transform.position += (Vector3.left * 0.5f);
                isFlipped = false;
            }

            RaycastHit2D hit;
            Move(horizontal, vertical, out hit);
        }
        else {
            animator.SetFloat("speed", 0.0f);
        }
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit) {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, newPostion, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null) {
            animator.SetFloat("speed", 1f);
            rb2D.MovePosition(newPostion);
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Door") {
            System.Console.Write("Door touched");
        }
    }
}
