using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("The container for label and value of the score UI text")]
    [SerializeField]
    private GameObject m_ScoreContainer;

    private TMP_Text m_ScoreValue;
    private float m_RoundTime = 0f;


    private void Start()
    {
        m_ScoreValue = m_ScoreContainer.GetComponentsInChildren<TMP_Text>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        m_RoundTime += Time.deltaTime;

        //m_ScoreValue.text = string.Format("{0} seconds", Math.Round(m_RoundTime, 2).ToString());
    }

    public void Score()
    {
        Debug.Log(string.Format("Round is finished in {0} seconds", Math.Round(m_RoundTime, 2).ToString()));
    }
}
