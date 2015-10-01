using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Seeker))]
public class Boss : MonoBehaviour {
    public enum Type {
        FIRE,
        WATER,
        AIR,
        EARTH,
        RAINBOW
    }

    private Seeker seeker;
    private Rigidbody2D rb;
    private Animator animator;
    private Type type = Type.RAINBOW;
    private float colorTime = 0f;
    private int rainbowIndex = 0;
    private Color[] rainbow = new Color[] { Color.red, new Color(1.0f, 0.5f, 0f), Color.yellow, Color.green, Color.blue, Color.cyan, Color.magenta };
    private float colorFadeDelay = 0.3f;

    public Transform target;
    public Path path;
    public float speed;
    public float updateRate = 2f;
    public ForceMode2D fMode;
    public float nextWaypointDistance = 3f;

    private int currentWaypoint = 0;

    [HideInInspector]
    public bool pathHasEnded = false;

    public Type BossType {
        get {
            return type;
        }
    }

    void Start() {
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null) {
            Debug.LogError("No player target found.");
            return;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);
        StartCoroutine(UpdatePath());

        v = transform.position - center.position;

        InitType();
    }

    private void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }


    private IEnumerator UpdatePath() {
        if (target == null || !GameManager.instance.IsBossAlive(type))
            yield return false;

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);

        StartCoroutine(UpdatePath());
    }

    void FixedUpdate() {
        if (!GameManager.instance.IsBossAlive(type))
            return;

        //FollowTarget();

        CircleMove();

        // TODO add different behaviors of AI here (ex. stop and shoot fireballs)
    }

    private void FollowTarget() {
        if (target == null || path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count) {
            if (pathHasEnded) return;

            pathHasEnded = true;
            return;
        }

        pathHasEnded = false;

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        rb.AddForce(dir, fMode);

        float horizontal = rb.velocity.x;
        float vertical = rb.velocity.y;

        animator.SetFloat("speed", Mathf.Abs(horizontal != 0 ? horizontal : vertical));

        if (horizontal > 0f && !facingRight)
            Flip();
        else if (horizontal < 0f && facingRight)
            Flip();

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist <= nextWaypointDistance) {
            currentWaypoint++;
            return;
        }
    }

    public Transform center;
    public float degreesPerSecond = -65.0f;
    private Vector3 v;
    private void CircleMove() {
        rb.isKinematic = true;
        v = Quaternion.AngleAxis(degreesPerSecond * Time.deltaTime, Vector3.forward) * v;
        transform.position = ((Vector3)GetComponent<Collider2D>().offset) + center.position + v;
        animator.SetFloat("speed", 1f);
    }

    private void InitType() {
        type =
            (GameManager.instance.RoomX == 1 && GameManager.instance.RoomY == 0) ? Type.FIRE :
            (GameManager.instance.RoomX == 0 && GameManager.instance.RoomY == 1) ? Type.WATER :
            (GameManager.instance.RoomX == 2 && GameManager.instance.RoomY == 1) ? Type.AIR :
            (GameManager.instance.RoomX == 1 && GameManager.instance.RoomY == 2) ? Type.EARTH :
            (GameManager.instance.RoomX == 1 && GameManager.instance.RoomY == 4) ? Type.RAINBOW : Type.RAINBOW;

        switch (type) {
            case Type.FIRE:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case Type.WATER:
                GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case Type.AIR:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case Type.EARTH:
                GetComponent<SpriteRenderer>().color = new Color(0.585f, 0.293f, 0f);
                break;
            case Type.RAINBOW:
                GetComponent<SpriteRenderer>().color = rainbow[0];
                break;
        }

        int health = GameManager.instance.GetBossHealth(type);
        animator.SetInteger("health", health);
        if (health < 0) {
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    void Update() {
        if (!GameManager.instance.IsBossAlive(type)) return;

        switch (type) {
            case Type.RAINBOW:
                RainbowFadeColor();
                break;
        }
    }

    private bool facingRight = false;
    void Flip() {
        facingRight = !facingRight;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        Vector3 position = transform.position;
        position.x += facingRight ? -2f : 2f;
        transform.position = position;
    }

    private void RainbowFadeColor() {
        int fromIndex = rainbowIndex;
        int toIndex = rainbowIndex + 1;

        if (toIndex >= rainbow.Length) toIndex = 0;

        GetComponent<SpriteRenderer>().color = Color.Lerp(rainbow[fromIndex], rainbow[toIndex], colorTime / colorFadeDelay);

        colorTime += Time.deltaTime;

        if (colorTime >= colorFadeDelay) {
            rainbowIndex++;

            if (rainbowIndex >= rainbow.Length) rainbowIndex = 0;

            colorTime = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "PlayerProjectile") {
            animator.SetTrigger("hit");
            GameManager.instance.DamageBoss(type, other.GetComponent<Projectile>().damage);
            animator.SetInteger("health", GameManager.instance.GetBossHealth(type));
            if (!GameManager.instance.IsBossAlive(type)) {
                GetComponent<CircleCollider2D>().enabled = false;
                rb.velocity = Vector3.zero;
                if (type == Type.RAINBOW) {
                    GetComponent<SpriteRenderer>().color = Color.white;
                    GameManager.instance.StartCoroutine(LoadVictoryScene());
                }
            }
        }
    }

    IEnumerator LoadVictoryScene() {
        SoundManager.instance.musicSource.Stop();
        SoundManager.instance.efxSource.Stop();

        yield return new WaitForSeconds(0.5f);

        Application.LoadLevel("Victory");
    }
}
