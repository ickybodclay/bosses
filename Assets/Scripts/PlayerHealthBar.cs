using UnityEngine;
using System.Collections.Generic;

public class PlayerHealthBar : MonoBehaviour {
    public GameObject player;
    public GameObject heartImage;

    private List<GameObject> hearts;

	void Start () {
        hearts = new List<GameObject>();
        UpdateHearts();
	}
	
	void Update () {
	    if(player.GetComponent<Player>().Health != hearts.Count) {
            UpdateHearts();
        }
	}

    private void UpdateHearts() {
        int playerHealth = player.GetComponent<Player>().Health;
        if (playerHealth > hearts.Count) {
            int offset = hearts.Count - 1;
            for(int i=0; i < playerHealth - hearts.Count; ++i) {
                GameObject heart = Instantiate(heartImage, this.transform.position + (Vector3.right * (offset + i)), Quaternion.identity) as GameObject;
                heart.transform.SetParent(this.transform);
                hearts.Add(heart);
            }
        }
        else if (playerHealth < hearts.Count) {
            if (playerHealth < 0) playerHealth = 0;
            while(hearts.Count > playerHealth) {
                Destroy(hearts[hearts.Count - 1].gameObject);
                hearts.RemoveAt(hearts.Count - 1);
            }
        }
    }
}
