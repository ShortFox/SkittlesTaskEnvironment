using System.Collections;
using UnityEngine;

public class SkittlesPlayer : Player
{
    protected PivotProperties _pivot;
    protected BallController _ball;

    private float _angularSpeed = -300f;
    private float _angularSpeedStepSize = 5f;

    public SkittlesPlayer(Environment env, Task task) : base(env, task)
    {
        try
        {
            _pivot = _environment.transform.Find("Pivot").GetComponent<PivotProperties>();
            _ball = _pivot.transform.Find("Ball").gameObject.GetComponent<BallController>();
        }
        catch
        {
            Debug.LogError("Failed to implement: " + this.GetType().Name);
        }
    }
    public override void ComputeState()
    {
        _pivot.transform.Rotate(0, _angularSpeed * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.Space) && !_ball.Detached) _ball.Detach();
        if (Input.GetKey(KeyCode.R) && _ball.Detached) _ball.Reattach("Reset");

        if (Input.GetKey(KeyCode.UpArrow))
            _angularSpeed = Mathf.Clamp(_angularSpeed + _angularSpeedStepSize, -600f, 600f);
        if (Input.GetKey(KeyCode.DownArrow))
            _angularSpeed = Mathf.Clamp(_angularSpeed - _angularSpeedStepSize, -600f, 600f);

        // Update Ball State
        _ball.UpdateState(Time.fixedDeltaTime);

        base.ComputeState();
    }
}
