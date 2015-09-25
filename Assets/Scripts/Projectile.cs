using UnityEngine;

public class Projectile : MonoBehaviour {
    public int damage;
    public float lifetime;
    public bool rotate;
    public GameObject target;

    void Start() {
        Destroy(this.gameObject, lifetime);
    }

    void Update() {
        if(rotate) transform.Rotate(Vector3.forward, -10f);
        //FlashRed();
    }

    private void FlashRed() {
        float r = Random.Range(0f, 1f);
        float g = 0f;
        float b = 0f;
        GetComponent<SpriteRenderer>().color = new Color(r, g, b);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Wall" || other.tag == "Door" || other.tag == target.tag) {
            Destroy(this.gameObject);
        }
    }
}
