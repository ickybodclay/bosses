using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
    public enum Spawn {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
        CENTER
    }

    public int destinationRoomX;
    public int destinationRoomY;
    public Spawn destinationSpawn;

}
