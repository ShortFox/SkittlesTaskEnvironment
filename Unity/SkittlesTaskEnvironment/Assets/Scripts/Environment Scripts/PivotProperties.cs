using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotProperties : MonoBehaviour
{
    public Vector3 Position { get; private set; }
    public Vector3 Angle { get; private set; }
    public float AngleVel { get; private set; }

    private Vector3 _oldRot;
    private Vector3 _rot;

    private float _direction;

    private void Start()
    {
        Position = this.transform.position;
        Angle = this.transform.eulerAngles;
        AngleVel = 0f;

        _oldRot = this.transform.forward;
        _rot = this.transform.forward;
    }

    public void UpdateState(float dt)
    {
        Angle = this.transform.eulerAngles;
        _rot = this.transform.forward;
        AngleVel = Vector3.SignedAngle(_oldRot, _rot, Vector3.up) / dt;
        _oldRot = _rot;

        GameStates.PivotAngle = Angle.y;
        GameStates.PivotAngVel = AngleVel;
    }
}
