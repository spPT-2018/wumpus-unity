using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private DateTime _started;

    public Timer()
    {
        Play();
    }

    public double Check()
    {
        return DateTime.UtcNow.Subtract(_started).Ticks * 100D;
    }

    public void Play()
    {
        _started = DateTime.UtcNow;
    }
}
