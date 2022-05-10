using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkittlesTask_Tutorial : SkittlesTask
{
    private SkittlesPlayer_HumanMouse _player;

    public SkittlesTask_Tutorial(Environment env) : base(env)
    {
        MyPlayer = new SkittlesPlayer_HumanMouse(env, this);
        _player = (SkittlesPlayer_HumanMouse)MyPlayer;

        UIManager.Instance.OnHideUI += Initiate;
    }

    private void SetStatus(string msg)
    {
        UIManager.Instance.StatusText.text = msg;
    }

    private void DisplayIntroduction()
    {
        string introduction =
            "Welcome to the experiment.\n"
            + "Your goal is to throw the <color=white>white ball</color> around the <color=red>red post</color> to hit the <color=yellow>yellow target</color>.\n"
            + "When thrown, the <color=white>white ball</color> is attracted to the <color=red>red post</color> (like a tetherball).";
        SetStatus(introduction);
        _player.Moveable = false;
        EventArguments.Instance.OnSpacebarDown += DisplayMovementInstructions;

    }
    private void DisplayMovementInstructions()
    {
        EventArguments.Instance.OnSpacebarDown -= DisplayMovementInstructions;
        string instruction =
            "The <color=white>white ball</color> is attached to the <color=fuchsia>pink arm</color>.\n"
            + "The <color=fuchsia>pink arm</color> can be rotated by moving your mouse cursor anywhere on the screen.\n"
            + "Please practice rotating the <color=fuchsia>pink arm</color> a full 360 degrees.";
        SetStatus(instruction);
        _player.Moveable = true;
        EventArguments.Instance.OnSpacebarDown += DisplayThrowingInstructions;
    }
    private void DisplayThrowingInstructions()
    {
        EventArguments.Instance.OnSpacebarDown -= DisplayThrowingInstructions;
        string instruction =
            "When you are ready to throw the <color=white>white ball</color>, hold down the primary mouse button and move your computer mouse.\n"
            + "To release the <color=white>white ball</color> from the <color=fuchsia>pink arm</color>, lift up from the mouse button.\n"
            + "Please practice throwing the <color=white>white ball</color> and try to hit the <color=yellow>yellow target</color>.";
        SetStatus(instruction);
        _player.Active = true;
        BallController.OnReattach += DisplayRedPostNotice;
    }
    private void DisplayRedPostNotice()
    {
        _player.Active = false;
        BallController.OnReattach -= DisplayRedPostNotice;
        string instruction =
            "Remember, the <color=white>white ball</color> is attracted to the <color=red>red post</color>.\n"
            + "To avoid hitting the <color=red>red post</color>, throw the <color=white>white ball</color> so that it moves around the post.\n"
            + "Throw the <color=white>white ball</color> again and try to hit the <color=yellow>yellow target</color>.";
        SetStatus(instruction);
        _player.Active = true;
        BallController.OnReattach += ResetBall;
        EventArguments.Instance.OnSpacebarDown += Finish;
    }
    private void ResetBall()
    {
        _player.Reset();
        _player.Active = true;
    }

    public override void Initiate()
    {
        Debug.Log("Starting Tutorial");
        UIManager.Instance.OnHideUI -= Initiate;
        if (ParticipantInfo.Session > 1) Finish();
        else
        {
            DisplayIntroduction();
        }
    }

    public override void Finish()
    {
        if (BallController.Detached) return;

        _player.Moveable = true;
        _player.Active = false;
        BallController.OnReattach -= ResetBall;
        EventArguments.Instance.OnSpacebarDown -= Finish;
        SetStatus("");
        base.Finish();
    }
}
