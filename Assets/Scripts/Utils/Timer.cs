using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class Timer
{
    private float m_EndTime;
    private float m_ElapsedTime = 0f;
    private bool m_IsRunning = false;
    private bool m_IsComplete;

    public bool IsComplete {  get { return m_IsComplete; } }
    public bool IsRunning { get { return m_IsRunning; } }

    // Update is called once per frame
    public void Update()
    {
        if (!m_IsRunning)
            return;

        m_ElapsedTime += Time.deltaTime;

        if (m_ElapsedTime >= m_EndTime)
        {
            EndTimer();
            m_IsComplete = true;
        }
    }

    public void StartTimer(float time)
    {
        if (m_IsRunning)
            return;

        m_IsRunning = true;
        m_EndTime = time;
        m_ElapsedTime = 0f;
        m_IsComplete = false;
    }

    public void EndTimer()
    {
        m_IsComplete = true;
        m_IsRunning = false;
    }
}
