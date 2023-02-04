using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Tooltip("The container for the tutorial UI elements")]
    [SerializeField]
    private GameObject m_TutorialContainer;

    [Tooltip("The text that shows instructions on progressing through tutorial")]
    [SerializeField]
    private TMP_Text m_TutorialHelperText;

    [Tooltip("The text which shows when tutorial is finished")]
    [SerializeField]
    private TMP_Text m_TutorialFinisherText;

    [Tooltip("The text which shows how many players there are")]
    [SerializeField]
    private TMP_Text m_PlayerCountValue;

    private List<TMP_Text> m_TutorialStages = new List<TMP_Text>();

    private int m_TutorialStage = 1;
    private int m_NumberOfPlayers = 1;

    // Start is called before the first frame update
    void Start()
    {
        m_NumberOfPlayers = PlayerPrefs.GetInt("PlayerCount", 1);
        m_TutorialStages = m_TutorialContainer.GetComponentsInChildren<TMP_Text>().Where(child => child.gameObject.CompareTag("Tutorial")).ToList();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button0))
        {
            AdvanceTutorial();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow)|| Input.GetKeyUp(KeyCode.Joystick1Button1))
            m_NumberOfPlayers++;
        else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.Joystick1Button2))
            m_NumberOfPlayers--;

        m_NumberOfPlayers = Mathf.Clamp(m_NumberOfPlayers, 1, 10);
        m_PlayerCountValue.text = m_NumberOfPlayers.ToString();

        UpdateVisibleTutorial();
    }

    void AdvanceTutorial()
    {
        if (m_TutorialStage == m_TutorialStages.Count - 1)
        {
            m_TutorialFinisherText.enabled = true;
            m_TutorialHelperText.enabled = false;
        } else if (m_TutorialStage == m_TutorialStages.Count)
        {
            // Move to next scene
            PlayerPrefs.SetInt("PlayerCount", m_NumberOfPlayers);
            SceneManager.LoadScene("NonVRGame");
        }

        m_TutorialStage++;
    }

    void UpdateVisibleTutorial()
    {
        if (m_TutorialStages.Count == 0)
            return;

        for (int iTutorial = 0; iTutorial < m_TutorialStages.Count; iTutorial++)
        {
            m_TutorialStages[iTutorial].enabled = m_TutorialStage == iTutorial + 1;
        }
    }
}
