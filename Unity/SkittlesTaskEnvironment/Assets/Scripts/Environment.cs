using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public Task MyTask { get; private set; }
    public Player MyPlayer { get; private set; }

    private void Awake()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = (1f / Screen.currentResolution.refreshRate);
    }
    private void Start()
    {
        MyTask = new SkittlesTask_Tutorial(this); // Only completes tutorial if Session Number = 1.
        MyTask.OnFinish += SelectTask;
    }
    void FixedUpdate()
    {
        if (MyTask != null) MyTask.CheckState();
    }

    private void SelectTask()
    {
        MyTask.OnFinish -= SelectTask;
        Debug.Log("Starting Experiment");

        // Implements a simple task with keyboard controls
        //MyTask = new SkittlesTask(this);

        // Implements task used in human validation. Also writes data to OutData.
        MyTask = new SkittlesTask_HumanRecordBlocks(this);

    }
}
