using UnityEngine;
public abstract class Task:ITask
{
    protected Environment _environment;
    public Player MyPlayer { get; protected set; }

    public Task(Environment env)
    {
        _environment = env;
    }

    #region Events
    //Event to signal experiment preperation
    public delegate void InitiateAction();
    public event InitiateAction OnInitiate;

    //Event to signal trial preparation.
    public delegate void SetupAction();
    public event SetupAction OnSetup;

    //Event to signal trial is currently running.
    public delegate void RunAction();
    public event RunAction OnRun;

    //Event to signal trial is done.
    public delegate void CleanupAction();
    public event CleanupAction OnCleanup;

    //Event to signal task is over
    public delegate void FinishAction();
    public event FinishAction OnFinish;
    #endregion

    #region Methods
    public virtual void Initiate()
    {
        OnInitiate?.Invoke();
    }
    public virtual void Setup()
    {
        OnSetup?.Invoke();
    }
    public virtual void Run()
    {
        OnRun?.Invoke();
    }
    public virtual void CheckState()
    {
        if (MyPlayer != null) MyPlayer.ComputeState();
    }
    public virtual void Cleanup()
    {
        OnCleanup?.Invoke();
    }
    public virtual void Finish()
    {
        OnFinish?.Invoke();
    }
    #endregion
}
