using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [Tooltip("A prefab potato for throwing")]
    [SerializeField]
    private GameObject m_Potato;

    private GameObject m_PotatoInstance;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_PotatoInstance != null && !m_PotatoInstance.activeInHierarchy)
        {
            m_PotatoInstance = null;
        }

        if (!m_PotatoInstance && Input.GetKeyUp(KeyCode.Space))
        {
            Vector3 newPosition = transform.position + new Vector3(0, 0.15f, 1.15f);
            Quaternion newRotation = transform.rotation;

            m_PotatoInstance = GameObject.Instantiate(m_Potato, newPosition, newRotation);
            m_PotatoInstance.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 400));
        }
    }
}
