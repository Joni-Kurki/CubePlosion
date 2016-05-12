using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

    void OnTriggeredEnter(Collider other) {
        Destroy(other.gameObject);
    }

    
}
