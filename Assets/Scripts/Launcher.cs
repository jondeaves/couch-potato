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

    [Tooltip("How long after throwing a potato before you can throw another")]
    [SerializeField]
    private float m_ThrowAgainDelay = 2f;

    [SerializeField]
    private GameManager m_GameManager;

    private Rigidbody m_Rigidbody;

    private float m_EyesDefaultY;
    private float m_EyeMovementRange = 0.2f;

    private float m_ChargeTimer = 0f;

    private List<GameObject> m_PotatoInstances;
    private Timer m_ThrowTimer= new Timer();

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody= GetComponent<Rigidbody>();
        m_EyesDefaultY = m_Eyes.transform.position.y;
        m_PotatoInstances =  new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_GameManager.IsPlaying)
            return;

        UpdateInput();
        UpdateThrow();

        m_ThrowTimer.Update();
    }

    private void UpdateThrow()
    {
        float chargedThrowPower = m_ChargeTimer / m_ChargeTime;

        if (chargedThrowPower > 0)
            m_SliderUI.gameObject.SetActive(true);
        else
            m_SliderUI.gameObject.SetActive(false);

        // No point in doing anything else if we are cooling down
        if (m_ThrowTimer.IsRunning)
            return;

        // Hold to charge a throw
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button0))
        {
            m_SliderUI.value = chargedThrowPower;
            m_ChargeTimer += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button0))
        {
            Debug.Log(string.Format("Throwing at {0}% of max power", (Mathf.Clamp(chargedThrowPower, 0.25f, 1f) * 100f).ToString()));
            m_ChargeTimer = 0f;
            m_SliderUI.value = 0f;

            GameObject pInstance = GameObject.Instantiate(m_Potato, m_Hand.transform.position, Quaternion.identity);
            pInstance.GetComponent<Rigidbody>().AddForce(m_Hand.transform.forward * m_ThrowPower * Mathf.Clamp(chargedThrowPower, 0.25f, 1f));

            m_PotatoInstances.Add(pInstance);

            m_ThrowTimer.StartTimer(m_ThrowAgainDelay);
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
