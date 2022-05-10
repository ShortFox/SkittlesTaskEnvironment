using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class SkittlesTask_HumanRecordBlocks : SkittlesTask
{
    public sealed class DataWriter
    {
        private static readonly DataWriter _instance = new DataWriter();
        public static DataWriter Instance { get { return _instance; } }

        public static List<string> Buffer;
        private static string _folderPath;

        private DataWriter()
        {
            Buffer = new List<string>();
            _folderPath = Application.dataPath + "/OutData";
        }
        public void CreateDirectory(int partID, int session)
        {
            _folderPath = Application.dataPath + $"/OutData/Participant {partID}/Session {session}";
            if (!Directory.Exists(_folderPath)) Directory.CreateDirectory(_folderPath);
        }

        public IEnumerator RecordTrial(int block, int trial)
        {
            Buffer.Clear();
            ParticipantInfo.Block = block;
            ParticipantInfo.TrialNum = trial;
            Buffer.Add(GameStates.GetHeader());

            while (true)
            {
                yield return new WaitForFixedUpdate();
                Buffer.Add(GameStates.GetString());
            }
        }

        public void WriteFile()
        {
            if (Buffer.Count == 0) return;

            string filename = ParticipantInfo.ToFileName();

            using (StreamWriter sw = File.AppendText(Path.Combine(_folderPath, filename)))
            {
                for (int i = 0; i < Buffer.Count; i++) sw.WriteLine(Buffer[i]);
                Buffer.Clear();
            }
        }

    }

    private SkittlesPlayer_HumanMouse _player;
    private int _currentBlock;
    private int _totalBlocks;
    private int _trialsPerBlock;
    private int _trialsPracticeLength;
    private int _currentTrial;

    private int _numberHit;

    private IEnumerator CurrentCoroutine;


    public SkittlesTask_HumanRecordBlocks(Environment env) : base(env)
    {
        MyPlayer = new SkittlesPlayer_HumanMouse(env, this);
        _player = (SkittlesPlayer_HumanMouse)MyPlayer;

        _trialsPracticeLength = 10;
        _trialsPerBlock = 50;
        _totalBlocks = 6;
        _currentBlock = 0;  // 0 = practice.
        _currentTrial = 1;
        _numberHit = 0;

        UIManager.Instance.OnHideUI += Initiate;
        //UIManager.Instance.OnShowUI += Finish;
        SetStatus("");
        if (!UIManager.Instance.IsVisible) Initiate();
    }

    private void SetStatus(string msg)
    {
        UIManager.Instance.StatusText.text = msg;
    }

    private void AddResult(string result, float minDist)
    {
        BallController.OnResult -= AddResult;
        ParticipantInfo.Result = result;

        if (result == "Hit")
        {
            _numberHit++;
        }
    }

    #region Override Methods
    public override void Initiate()
    {
        DataWriter.Instance.CreateDirectory(ParticipantInfo.Number, ParticipantInfo.Session);

        UIManager.Instance.OnHideUI -= Initiate;
        SetStatus($"You will complete {_trialsPracticeLength} practice trials.\n"
            +$"You will then complete {_totalBlocks} blocks each consisting of {_trialsPerBlock} trials.\n"
            +"After each block, you will have an opportunity to take a short break.\n"
            +"Press [SPACEBAR] when you are ready to begin the practice trials.");
            
        EventArguments.Instance.OnSpacebarDown += Setup;

        base.Initiate();
    }

    public override void Setup()
    {
        EventArguments.Instance.OnSpacebarDown -= Setup;

        if (_currentBlock == 0)
        {
            if (_currentTrial <= _trialsPracticeLength) Run();
            else { Debug.LogError("ERROR: Current trial exceeds allowable for practice."); }
        }
        else if (_currentBlock <= _totalBlocks)
        {
            if (_currentTrial <= _trialsPerBlock) Run();
            else { Debug.LogError("ERROR: Current trial exceeds allowable for block."); }
        }
        else Debug.LogError("ERROR: Current block exceeds allowable for experiment.");

        base.Setup();
    }
    public override void Run()
    {
        SetStatus("");
        _player.Active = true;

        BallController.OnResult += AddResult;
        BallController.OnReattach += Cleanup;

        if (CurrentCoroutine != null)
        {
            _environment.StopCoroutine(CurrentCoroutine);
            CurrentCoroutine = null;
        }
        CurrentCoroutine = DataWriter.Instance.RecordTrial(_currentBlock,_currentTrial);
        _environment.StartCoroutine(CurrentCoroutine);

        base.Run();
    }
    public override void CheckState()
    {
        base.CheckState(); // Player state is updated here
    }
    public override void Cleanup()
    {
        _player.Active = false;
        BallController.OnReattach -= Cleanup;

        if (CurrentCoroutine != null)
        {
            _environment.StopCoroutine(CurrentCoroutine);
            CurrentCoroutine = null;
        }

        DataWriter.Instance.WriteFile();

        _currentTrial++;

        if (_currentBlock == 0)
        {
            if (_currentTrial > _trialsPracticeLength)
            {
                SetStatus("You have completed the practice trials.\nPress [SPACEBAR] to start the first block.");
                _currentTrial = 1;
                _currentBlock++;
            }
        }
        else if (_currentBlock <= _totalBlocks)
        {
            if (_currentTrial > _trialsPerBlock)
            {
                SetStatus($"You have completed block {_currentBlock}/{_totalBlocks}.\nPress [SPACEBAR] to start the next block.");
                _currentTrial = 1;
                _currentBlock++;
            }
        }
        base.Cleanup();

        if (_currentBlock > _totalBlocks) { Finish(); }
        else if (_currentTrial == 1)
        {
            EventArguments.Instance.OnSpacebarDown += Setup;
        }
        else
        {
            Setup();
        }
    }
    public override void Finish()
    {
        SetStatus($"You have completed the session.\nYour score is: {_numberHit}.\nThank you for your participation.");
        UIManager.Instance.OnShowUI -= Finish;
        _player.Active = false;

        base.Finish();
    }
    #endregion
}
