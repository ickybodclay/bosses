using UnityEngine;

public class Boss : MonoBehaviour {
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();

        //float r = Random.Range(0f, 1f);
        //float g = Random.Range(0f, 1f);
        //float b = Random.Range(0f, 1f);
        //ChangeColor(r, g, b);
    }

    private void ChangeColor(float r, float g, float b) {
        GetComponent<SpriteRenderer>().color = new Color(r, g, b);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "PlayerProjectile") {
            animator.SetTrigger("hit");
        }
    }
}
