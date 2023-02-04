using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

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

    [Tooltip("How hard you throw")]
    [SerializeField]
    private float m_ThrowPower = 400f;

    [Tooltip("How fast the players head moves up and down")]
    [SerializeField]
    private float m_LookSpeed = 10f;

    [Tooltip("How long, in seconds, you need to hold button to get max throwing power")]
    [SerializeField]
    private float m_ChargeTime = 1f;

    [Tooltip("The slider UI component to visually show charged power")]
    [SerializeField]
    private Slider m_SliderUI;

    private GameObject m_PotatoInstance;
    private Rigidbody m_Rigidbody;

    private float m_EyesDefaultY;
    private float m_EyeMovementRange = 0.2f;

    private float m_ChargeTimer = 0f;

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
        UpdateThrow();
    }

    private void UpdateThrow()
    {
        if (m_PotatoInstance != null && !m_PotatoInstance.activeInHierarchy)
        {
            m_PotatoInstance = null;
        }
        else if (m_PotatoInstance != null)
        {
            return;
        }

        // Hold to charge a throw
        float chargedThrowPower = m_ChargeTimer / m_ChargeTime;
        m_SliderUI.value = 0f;
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button0))
        {
            m_SliderUI.value = chargedThrowPower;
            m_ChargeTimer += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button0))
        {
            Debug.Log(string.Format("Throwing at {0}% of max power by holding for {1} seconds", (Mathf.Clamp(chargedThrowPower, 0.25f, 1f) * 100f).ToString(), m_ChargeTimer.ToString()));
            m_ChargeTimer = 0f;

            m_PotatoInstance = GameObject.Instantiate(m_Potato, m_Hand.transform.position, Quaternion.identity);
            m_PotatoInstance.GetComponent<Rigidbody>().AddForce(m_Hand.transform.forward * m_ThrowPower * chargedThrowPower);
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

            // Rotate hand based on camera angle
            Quaternion newRot = Quaternion.Euler(
                GameObject.FindObjectOfType<CinemachineVirtualCamera>().gameObject.transform.rotation.eulerAngles.x,
                m_Hand.transform.rotation.eulerAngles.y,
                m_Hand.transform.rotation.eulerAngles.z
                );

            m_Hand.transform.rotation = newRot;
        }
    }
}
