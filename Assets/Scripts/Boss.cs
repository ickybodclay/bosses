using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "PlayerProjectile") {
            animator.SetTrigger("hit");
        }
    }
}
