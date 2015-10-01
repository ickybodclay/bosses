using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour {

    public GameObject axeProjectile;
    public LayerMask blockingLayer;
    public float shootSpeed = 20f;
    public float shootDelay = 0.5f;
    public float takeDamageDelay = 0.5f;

    private Animator animator;
    private Rigidbody2D rb2D;

    private bool facingRight = true;
    private float maxSpeed = 300f;
    private float lastShootTime;
    private float lastDamageTime = 0f;
    private Vector3 center;

    void Start() {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        center = GetComponent<BoxCollider2D>().offset;
        Spawn(GameManager.instance.Spawn);
    }

    private void Spawn(Door.Spawn spawn) {
        Vector3 spawnPosition = transform.position;
        switch (spawn) {
            case Door.Spawn.TOP:
                spawnPosition.x = 4.7f;
                spawnPosition.y = 0f;
                break;
            case Door.Spawn.BOTTOM:
                spawnPosition.x = 4.7f;
                spawnPosition.y = 5.5f;
                break;
            case Door.Spawn.LEFT:
                spawnPosition.x = 0f;
                spawnPosition.y = 3.5f;
                break;
            case Door.Spawn.RIGHT:
                spawnPosition.x = 9f;
                spawnPosition.y = 3.5f;
                Flip();
                break;
            case Door.Spawn.CENTER:
                spawnPosition.x = 3.5f;
                spawnPosition.y = 3.5f;
                break;
        }
        transform.position = spawnPosition;
    }

    void Update() {
        if (GameManager.instance.PlayerHealth <= 0) return;

        if (Input.GetKey(KeyCode.Z) || Input.GetButton("Fire1")) {
            Shoot();
        }

        float horizontal = 0f;
        float vertical = 0f;

        horizontal = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        vertical = Input.GetAxisRaw("Vertical") * Time.deltaTime;

        animator.SetFloat("speed", Mathf.Abs(horizontal != 0f ? horizontal : vertical) * maxSpeed);

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
        Vector3 position = transform.position;
        position.x += facingRight ? -0.5f : 0.5f;
        transform.position = position;
        center.x *= -1;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Door") {
            Door door = other.GetComponent<Door>();
            GameManager.instance.RoomX = door.destinationRoomX;
            GameManager.instance.RoomY = door.destinationRoomY;
            GameManager.instance.Spawn = door.destinationSpawn;
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Boss" && GameManager.instance.IsBossAlive(other.gameObject.GetComponent<Boss>().BossType)) {
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage) {
        if(damage > 0 && Time.time > (lastDamageTime + takeDamageDelay)) {
            animator.SetTrigger("hit");
            GameManager.instance.PlayerHealth -= damage;
            animator.SetInteger("health", GameManager.instance.PlayerHealth);
            if (GameManager.instance.PlayerHealth <= 0) StartCoroutine(LoadGameOver());
            lastDamageTime = Time.time;
        }
    }

    IEnumerator LoadGameOver() {
        rb2D.velocity = Vector2.zero;
        SoundManager.instance.musicSource.Stop();
        SoundManager.instance.efxSource.Stop();

        yield return new WaitForSeconds(1f);

        GameManager.instance.ResetGame();
    }

    void Shoot() {
        if(Time.time > (lastShootTime + shootDelay)) {
            animator.SetTrigger("punch");
            GameObject projectile = Instantiate(axeProjectile, transform.position + center, Quaternion.identity) as GameObject;
            projectile.GetComponent<Rigidbody2D>().velocity = (facingRight ? Vector2.right : Vector2.left) * shootSpeed;
            lastShootTime = Time.time;
        }
    }
}
