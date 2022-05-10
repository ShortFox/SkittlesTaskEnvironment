using UnityEngine;

public class SkittlesTask : Task
{
    public GameObject CenterPost { get; private set; }
    public GameObject Pivot { get; private set; }
    public GameObject Ball { get; private set; }
    protected BallController BallController;

    public SkittlesTask(Environment env):base(env)
    {
        try
        {
            CenterPost = _environment.transform.Find("Center Post").gameObject;
            Pivot = _environment.transform.Find("Pivot").gameObject;
            Ball = Pivot.transform.Find("Ball").gameObject;
            BallController = Ball.GetComponent<BallController>();
            if (BallController == null) Debug.LogError("Error: Ball does not have BallController component");
            if (MyPlayer == null) MyPlayer = new SkittlesPlayer(_environment, this);

        }
        catch
        {
            Debug.LogError("Failed to implement SkittlesTask");
        }
    }
    public override void Initiate()
    {
        base.Initiate();
    }
    public override void Setup()
    {
        base.Setup();
    }
    public override void Run()
    {
        base.Run();
    }
    private void Cleanup(bool result)
    {
        Cleanup();
    }
    public override void Cleanup()
    {
        base.Cleanup();
    }
    public override void Finish()
    {
        base.Finish();
    }
}
