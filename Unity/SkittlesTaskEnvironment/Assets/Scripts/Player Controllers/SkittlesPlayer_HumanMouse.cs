using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkittlesPlayer_HumanMouse : SkittlesPlayer
{

    protected Vector3 mousePos;

    private bool _releaseable;

    // Event to signal when ready
    //Event to signal trial preparation.

    public delegate void ReadyAction();
    public event ReadyAction OnReady;

    public bool Moveable;

    public override bool Active
    {
        get
        {
            return base.Active;
        }
        set
        {
            if (base.Active != value)
            {
                if (value)
                {
                    Moveable = true;
                    EventArguments.Instance.OnMouseDown += SetReady;
                    EventArguments.Instance.OnMouseUp += Release;
                }
                else
                {
                    Moveable = false;
                    EventArguments.Instance.OnMouseDown -= SetReady;
                    EventArguments.Instance.OnMouseUp -= Release;
                }
            }
            base.Active = value;
        }
    }

    public SkittlesPlayer_HumanMouse(Environment env, Task task) : base(env, task) 
    {
        _releaseable = false;
        Moveable = false;
    }

    protected void SetReady()
    {
        EventArguments.Instance.OnMouseDown -= SetReady;
        _releaseable = true;
        Cursor.visible = false;

    }
    protected void Release()
    {
        if (!_releaseable) return;

        EventArguments.Instance.OnMouseUp -= Release;
        _ball.Detach();
        _releaseable = false;
        Cursor.visible = true;
    }

    public override void ComputeState()
    {
        if (!Moveable) return;

        // Update Mouse Information.
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y-_pivot.transform.position.y));
        GameStates.MousePos = new Vector2(mousePos.x, mousePos.z);
        GameStates.MouseDown = Input.GetMouseButton(0);
        
        // Update Pivot Arm Information
        _pivot.transform.LookAt(mousePos, Vector3.up);
        _pivot.transform.eulerAngles = new Vector3(0f, _pivot.transform.eulerAngles.y, 0f);
        _pivot.UpdateState(Time.fixedDeltaTime);

        // Update Ball State
        _ball.UpdateState(Time.fixedDeltaTime);
    }

    public override void Reset()
    {
        Cursor.visible = true;
        Active = false;
        Moveable = false;
    }

}
