using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private Transform _canvas;
    public Text TrialCount { get; private set; }
    public Text SessionField { get; private set; }
    public Text ParticipantField { get; private set; }
    public Text StatusText { get; private set; }

    public Action OnHideUI;
    public Action OnShowUI;
    public bool IsVisible { get; private set; }

    private bool _validData = false;

    private void Awake()
    {
        Instance = this;
        _canvas = this.transform.Find("Canvas");

        TrialCount = _canvas.Find(nameof(TrialCount)).GetComponent<Text>();
        SessionField = _canvas.Find(nameof(SessionField)).Find("Text").GetComponent<Text>();
        ParticipantField = _canvas.Find(nameof(ParticipantField)).Find("Text").GetComponent<Text>();
        StatusText = _canvas.Find(nameof(StatusText)).GetComponent<Text>();
        IsVisible = true;

        EventArguments.Instance.OnH += HideUI;
        EventArguments.Instance.OnS += ShowUI;
    }

    private void OnGUI()
    {
        if (!String.IsNullOrEmpty(SessionField.text) && !String.IsNullOrEmpty(ParticipantField.text))
        {
            if (int.Parse(SessionField.text) >= 0 && int.Parse(ParticipantField.text) >= 0)
            {
                _validData = true;
                ParticipantInfo.Session = int.Parse(SessionField.text);
                ParticipantInfo.Number = int.Parse(ParticipantField.text);
            }
            else _validData = false;
        }
    }

    private void HideUI()
    {
        if (!_validData) return;

        SessionField.transform.parent.gameObject.SetActive(false);
        ParticipantField.transform.parent.gameObject.SetActive(false);
        TrialCount.transform.gameObject.SetActive(false);
        IsVisible = false;
        OnHideUI?.Invoke();
    }
    private void ShowUI()
    {
        SessionField.transform.parent.gameObject.SetActive(true);
        ParticipantField.transform.parent.gameObject.SetActive(true);
        TrialCount.transform.gameObject.SetActive(true);
        IsVisible = true;
        OnShowUI?.Invoke();
    }

}
