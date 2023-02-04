using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The number of seconds before the potato is considered dead")]
    private float m_TTL = 3f;

    private float m_TimeAlive = 0f;

    // Start is called before the first frame update
    void Start()
    {
        m_TimeAlive = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        m_TimeAlive += Time.deltaTime;

        // Maybe change this to be related to speed?
        if (m_TimeAlive >= m_TTL)
        {
            Destroy(gameObject);
        }
    }
}
