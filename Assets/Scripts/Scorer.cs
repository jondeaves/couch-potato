using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Potato") && other.transform.position.y < -0.7f) {
            Debug.Log("Goaaaaal");
            Debug.Log(other.transform.position.y);

            Destroy(other.gameObject);
        }
    }
}
