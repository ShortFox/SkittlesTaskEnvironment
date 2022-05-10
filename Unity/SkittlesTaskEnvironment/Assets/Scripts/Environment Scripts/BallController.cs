using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public bool Detached { get; private set; }

    private Transform _target;
    private Transform _pivot;
    private Transform _centerPost;
    private Vector3 _initLocalPos;
    private Coroutine _myCuroutine;

    // Ball Dynamics (tetherball-like dynamics)
    private const float stiffness = 1;
    private const float damping = 0.01f;
    private const float mass = 0.1f;
    private const float _hitCriteria = 0.05f;
    private const float _timeToReset = 2f;

    private Vector3 _acceleration = Vector3.zero;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _oldPosition = Vector3.zero;
    private Vector3 _position = Vector3.zero;

    public Vector3 Position { get { return _position; } }
    public Vector3 Velocity { get { return _velocity; } }

    private Vector3 _offset = Vector3.zero;

    private float _minDistToTarget = float.PositiveInfinity;
    private float _distanceToTarget = float.PositiveInfinity;

    public delegate void ReattachEvent();
    public event ReattachEvent OnReattach;

    public delegate void ResultEvent(string result, float minDist);
    public event ResultEvent OnResult;

    public delegate void DetachEvent();
    public event DetachEvent OnDetach;

    private void Awake()
    {
        _pivot = this.transform.parent;
        if (_pivot.name != "Pivot") Debug.LogError("Failure to find Pivot");
        _pivot.transform.eulerAngles = Vector3.zero;

        _target = _pivot.parent.Find("Target");
        if (_target == null) Debug.LogError("Failure to find Target");

        _centerPost = _pivot.parent.Find("Center Post");
        if (_centerPost == null) Debug.LogError("Failure to find Center Post");
        _offset = _centerPost.transform.position;

        _initLocalPos = this.transform.localPosition;
        Detached = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Center Post") Reattach("Fail");
    }

    public void UpdateState(float dt)
    {
        if (!Detached)
        {
            _oldPosition = _position;
            _position = this.transform.position;
            _velocity = (_position - _oldPosition) / dt;
        }

        _distanceToTarget = Vector3.Distance(this.transform.position, _target.position);
        if (_distanceToTarget < _minDistToTarget) _minDistToTarget = _distanceToTarget;

        GameStates.BallPos = this.transform.position;
        GameStates.BallVel = _velocity;
        GameStates.Detached = Detached;
        GameStates.DistToTarget = _distanceToTarget;
    }

    public void Detach()
    {
        if (Detached) return;

        if (_myCuroutine != null) Debug.LogError("BallTrajectory running when not expected.");

        Detached = true;
        this.transform.parent = _pivot.parent;
        _myCuroutine = StartCoroutine(BallTrajectory());

        OnDetach?.Invoke();
    }

    public void Reattach(string result)
    {
        if (!Detached) return;
        Detached = false;

        OnResult?.Invoke(result, _minDistToTarget);

        StopCoroutine(_myCuroutine);
        _myCuroutine = null;

        _acceleration = Vector3.zero;
        _velocity = Vector3.zero;

        this.transform.parent = _pivot;
        this.transform.localPosition = _initLocalPos;
        _oldPosition = this.transform.position;
        _position = this.transform.position;
        _minDistToTarget = float.PositiveInfinity;
        _distanceToTarget = float.PositiveInfinity;
        
        OnReattach?.Invoke();
    }

    public void SetState(Vector3 oldAng, Vector3 newAng)
    {
        _pivot.transform.eulerAngles = oldAng;
        _oldPosition = this.transform.position;

        _pivot.transform.eulerAngles = newAng;
        _position = this.transform.position;

        _velocity = (_position - _oldPosition) / Time.fixedDeltaTime;
    }

    public void SetInitialConditions(Vector3 pos, Vector3 vel)
    {
        if (Detached) return;

        _position = pos;
        _position -= _offset;
        _velocity = vel;
        Detach();
    }

    IEnumerator BallTrajectory()
    {
        float counter = 0;

        while (counter < _timeToReset)
        {
            _acceleration = (-damping * _velocity - stiffness * (_position- _offset)) / mass;
            _acceleration.y = 0f;
            _velocity += _acceleration * Time.fixedDeltaTime;
            _velocity.y = 0f;
            _position += _velocity * Time.fixedDeltaTime;
            this.transform.position = _position;
            counter  +=Time.fixedDeltaTime;

            if (_minDistToTarget <= _hitCriteria)
            {
                Reattach("Hit");
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        Reattach("Miss");
    }
}
