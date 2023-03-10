using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Tooltip("The object that controls launcher location")]
    [SerializeField]
    private GameObject m_Launcher;

    [Tooltip("The container for label and value of the score UI text")]
    [SerializeField]
    private GameObject m_ScoreContainer;

    [Tooltip("The label for who is up next")]
    [SerializeField]
    private GameObject m_NextUpContainer;

    [Tooltip("The panel overlay for complete round")]
    [SerializeField]
    private GameObject m_CompleteContainer;

    [Tooltip("The container for the victory screen scores")]
    [SerializeField]
    private GameObject m_ScoreLabels;

    [SerializeField]
    private TMP_Text m_WinnerLabel;

    [SerializeField]
    private AudioClip m_DingSoundClip;

    private TMP_Text m_ScoreValue;
    
    private float m_RoundTime = 0f;
    private int m_LastActivePlayer = -1;
    private int m_ActivePlayer = -1;

    private float[] m_PlayerScores;
    private int m_PlayersCompletedRound = 0;

    public bool IsPlaying {  get { return m_ActivePlayer !=-1; } }
    public float RoundTimer { get { return m_RoundTime; } }

    private void Start()
    {
        m_PlayerScores = new float[PlayerPrefs.GetInt("PlayerCount", 1)];
        m_ScoreValue = m_ScoreContainer.GetComponentsInChildren<TMP_Text>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlaying)
        {
            m_NextUpContainer.SetActive(false);
            m_CompleteContainer.SetActive(false);
            m_RoundTime += Time.deltaTime;
            m_ScoreValue.text = string.Format("{0} seconds", ((int)m_RoundTime).ToString());
        }
        else if (m_PlayersCompletedRound >= m_PlayerScores.Length) {
            m_NextUpContainer.SetActive(false);
            m_CompleteContainer.SetActive(true);
            TMP_Text[] scoreLabels = m_ScoreLabels.GetComponentsInChildren<TMP_Text>();

            float lowestScore = -1;
            int lowestScorePlayer = 1;

            for (int iScore = 0; iScore < scoreLabels.Count(); iScore++)
            {
                TMP_Text scoreLabel = scoreLabels[iScore];

                if (iScore < m_PlayerScores.Length)
                {
                    scoreLabel.gameObject.SetActive(true);
                    scoreLabel.text = string.Format("Player {0}: {1}", iScore+1, Math.Round(m_PlayerScores[iScore], 1).ToString());

                    if (lowestScore == -1 || lowestScore > m_PlayerScores[iScore])
                    {
                        lowestScore = m_PlayerScores[iScore];
                        lowestScorePlayer = iScore + 1;
                    }
                } else
                {
                    scoreLabel.gameObject.SetActive(false);
                }
            }

            m_WinnerLabel.text = string.Format("Player {0}", lowestScorePlayer.ToString());


            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button0))
            {
                SceneManager.LoadScene("TutorialScene");
            }
        } else 
        {
            int nextPlayerDue = m_LastActivePlayer == -1 ? 1 : (m_LastActivePlayer + 1);

            m_CompleteContainer.SetActive(false);
            m_NextUpContainer.SetActive(true);
            m_NextUpContainer.GetComponentsInChildren<TMP_Text>()[0].text = string.Format("Player {0} is up next", nextPlayerDue);

            // Delete the current 'taters
            
            Potato[] potatoes = FindObjectsOfType<Potato>();
            FindObjectsOfType<Potato>().ToList().ForEach(potato => Destroy(potato.gameObject));

            if(Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button0))
            {
                // Randomize player position and direction
                var vector2 = UnityEngine.Random.insideUnitCircle.normalized * 8f;
                m_Launcher.transform.position = new Vector3(vector2.x, 0, vector2.y);

                var lookPos = Vector3.zero - m_Launcher.transform.position;
                lookPos.y = 0;
                m_Launcher.transform.rotation = Quaternion.LookRotation(lookPos) * Quaternion.Euler(0, UnityEngine.Random.Range(-60, 60), 0);


                // Change to new player
                m_LastActivePlayer = m_ActivePlayer;
                m_ActivePlayer = nextPlayerDue;
            }
        }
    }

    public void Score()
    {
        Debug.Log(string.Format("Round is finished in {0} seconds", Math.Round(m_RoundTime, 2).ToString()));

        m_PlayerScores[m_ActivePlayer-1] = m_RoundTime;
        
        m_RoundTime = 0;
        m_PlayersCompletedRound++;

        m_LastActivePlayer = m_ActivePlayer;
        m_ActivePlayer = -1;

        GetComponent<AudioSource>().PlayOneShot(m_DingSoundClip);
    }
}
