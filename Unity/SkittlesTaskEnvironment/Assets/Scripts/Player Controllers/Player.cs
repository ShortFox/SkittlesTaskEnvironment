using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : IPlayer
{
    protected Environment _environment;
    protected Task _task;

    private bool _active;
    public virtual bool Active
    { 
        get 
        {
            return _active; 
        }
        set
        {
            _active = value;
        }
    }

    public Player(Environment env, Task task)
    {
        _environment = env;
        _task = task;
        Active = false;
        _task.OnCleanup += Reset;
    }

    public virtual void ComputeState()
    {
        if (!Active) return;
    }
    public virtual void Reset()
    {

    }
}