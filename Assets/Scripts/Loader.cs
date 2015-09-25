using UnityEngine;

public class Loader : MonoBehaviour {
    public GameObject gameManager;
    public GameObject soundManager;

    void Awake() {
        if (SoundManager.instance == null) {
            Instantiate(soundManager);
        }

        if (GameManager.instance == null) {
            Instantiate(gameManager);
        }
    }
}
