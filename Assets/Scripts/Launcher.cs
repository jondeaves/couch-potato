using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [Tooltip("A prefab potato for throwing")]
    [SerializeField]
    private GameObject m_Potato;

    [Tooltip("The object that the Camera looks at")]
    [SerializeField]
    private GameObject m_Eyes;

    [Tooltip("The object that the Potato is released from")]
    [SerializeField]
    private GameObject m_Hand;

    [Tooltip("How fast the player rotates")]
    [SerializeField]
    private float m_TurnSpeed= 200f;

    [Tooltip("How fast the players head moves up and down")]
    [SerializeField]
    private float m_LookSpeed = 10f;

    private GameObject m_PotatoInstance;
    private Rigidbody m_Rigidbody;

    private float m_EyesDefaultY;
    private float m_EyeMovementRange = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody= GetComponent<Rigidbody>();
        m_EyesDefaultY = m_Eyes.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        if (m_PotatoInstance != null && !m_PotatoInstance.activeInHierarchy)
        {
            m_PotatoInstance = null;
        }

        if (!m_PotatoInstance && Input.GetAxis("Fire1") > 0f)
        {
            m_PotatoInstance = GameObject.Instantiate(m_Potato, m_Hand.transform.position, Quaternion.identity);
            m_PotatoInstance.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * 400f);
        }
    }

    private void UpdateInput()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            m_Rigidbody.rotation *= Quaternion.AngleAxis(Input.GetAxis("Horizontal") * Time.deltaTime * m_TurnSpeed, Vector3.up);
        }

        if (Input.GetAxis("Vertical") != 0) {
            // Clamp to default +/- m_EyeMovementRange
            float updatedY = Mathf.Clamp(m_Eyes.transform.position.y + (0.1f * Input.GetAxis("Vertical") * Time.deltaTime * m_LookSpeed), m_EyesDefaultY - m_EyeMovementRange, m_EyesDefaultY + m_EyeMovementRange);

            m_Eyes.transform.position = new Vector3(
                m_Eyes.transform.position.x,
                updatedY,
                m_Eyes.transform.position.z);
        }
    }
}
