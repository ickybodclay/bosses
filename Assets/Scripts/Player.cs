using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float moveTime = 0.1f;
    public float inverseMoveTime;

    public LayerMask blockingLayer;

    private Animator animator;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;

    private bool facingRight = true;
    private float maxSpeed = 10f;

    void Start() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime;
    }

    void FixedUpdate() {
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        animator.SetFloat("speed", Mathf.Abs(horizontal != 0 ? horizontal : vertical));

        rb2D.velocity = new Vector2(horizontal * maxSpeed, vertical * maxSpeed);

        if (horizontal > 0 && !facingRight)
            Flip();
        else if (horizontal < 0 && facingRight)
            Flip();
    }

    void Flip() {
        facingRight = !facingRight;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Door") {
            Door door = other.GetComponent<Door>();
            GameManager.instance.RoomX = door.destinationRoomX;
            GameManager.instance.RoomY = door.destinationRoomY;
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
