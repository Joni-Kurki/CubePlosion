using UnityEngine;
using System.Collections;

public class CheckIfTouching : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnCollisionEnter(Collision other) {
        Debug.Log("Entering to" + other.collider.name + " with -> "+this.name);
    }
    void OnCollisionExit(Collision other) {
        Debug.Log("Exiting from"+  other.collider.name+ " with ->" + this.name);
    }
}
