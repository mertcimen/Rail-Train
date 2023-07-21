using System;
using System.Diagnostics;
using System.Threading;

public class CustomThread
{
    private bool isOn;

    private bool isStopped = false;

    private Thread thread;

    private int counter = 0;

    public CustomThread(Action action)
    {
        thread = new Thread(() => this.actionLoop(action));
    }

    public CustomThread(Action action, bool loop)
    {
        if (loop)
        {
            thread = new Thread(() => this.actionLoop(action));
        }
        else
        {
            thread = new Thread(() => this.action(action));
        }
    }

    public void StartOnceAfterTime(int timeInSeconds)
    {
        int val = timeInSeconds;
        while (val != 0)
        {
            val--;
            Thread.Sleep(1000);
            counter++;
        }

        counter = 0;
        thread.Start();
    }

    public void Start()
    {
        isOn = true;
        thread.Start();
    }

    public void Stop()
    {
        isStopped = true;
    }

    public bool IsOn()
    {
        return isOn;
    }

    private void action(Action action)
    {
        var sw = Stopwatch.StartNew();
        // while (isOn && !isStopped)
        // {
        action.Invoke();
        // }
        //
        // isOn = false;
        sw.Stop();
    }

    private void actionLoop(Action action)
    {
        var sw = Stopwatch.StartNew();
        while (isOn && !isStopped)
        {
            action.Invoke();
        }

        isOn = false;
        sw.Stop();
    }
}