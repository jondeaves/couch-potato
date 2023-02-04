using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("Updates when player scores")]
    [SerializeField]
    private TMP_Text m_ScoreValue;

    private float m_PlayerScore = 0;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Score()
    {
        m_PlayerScore++;
        m_ScoreValue.text = m_PlayerScore.ToString();
    }
}
