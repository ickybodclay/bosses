using UnityEngine;

public class Boss : MonoBehaviour {
    public enum Type {
        FIRE,
        WATER,
        AIR,
        EARTH,
        RAINBOW
    }

    private Animator animator;
    private Type type = Type.RAINBOW;

    public Type BossType {
        get {
            return type;
        }
    }

    void Start() {
        animator = GetComponent<Animator>();
        InitType();
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

        if (!GameManager.instance.IsBossAlive(type))
            Destroy(gameObject);
    }

    void Update() {
        switch (type) {
            case Type.FIRE:
                UpdateFireAI();
                break;
            case Type.WATER:
                break;
            case Type.AIR:
                break;
            case Type.EARTH:
                break;
            case Type.RAINBOW:
                RainbowFadeColor();
                break;
        }
    }

    private void UpdateFireAI() {

    }

    private float colorTime = 0f;
    private int rainbowIndex = 0;
    private Color[] rainbow = new Color[] { Color.red, new Color(1.0f, 0.5f, 0f), Color.yellow, Color.green, Color.blue, Color.cyan, Color.magenta };
    private float colorFadeDelay = 0.3f;

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
        if(other.tag == "PlayerProjectile") {
            animator.SetTrigger("hit");
            GameManager.instance.DamageBoss(type, other.GetComponent<Projectile>().damage);
            if(!GameManager.instance.IsBossAlive(type)) {
                Destroy(gameObject);
            }
        }
    }
}
