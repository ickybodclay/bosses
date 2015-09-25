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
    private Type type = Type.EARTH;

    void Start() {
        animator = GetComponent<Animator>();
        InitType();
    }

    private void InitType() {
        switch(type) {
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
    }

    void Update() {
        if(type == Type.RAINBOW) RainbowFadeColor();
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
        }
    }
}
