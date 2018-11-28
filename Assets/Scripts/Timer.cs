using System;
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
        Debug.Log($"Started: {_started.Ticks}, now: {DateTime.UtcNow.Ticks}");
        return DateTime.UtcNow.Subtract(_started).Ticks * 100D;
    }

    public void Play()
    {
        _started = DateTime.UtcNow;
    }
}
