using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorer : MonoBehaviour
{
    [SerializeField]
    GameManager m_GameManager;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Potato") && collision.transform.position.y < -0.7f)
        {
            Destroy(collision.gameObject);
            this.m_GameManager.Score();
        }
    }
}
