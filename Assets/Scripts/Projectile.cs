using UnityEngine;

public class Projectile : MonoBehaviour {
    public int damage;
    public float lifetime;

    void Start() {
        Destroy(this.gameObject, lifetime);
    }

    void Update() {
        transform.Rotate(Vector3.forward, -10f);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Wall" || other.tag == "Door" || other.tag == "Boss") {
            Destroy(this.gameObject);
        }
    }
}
