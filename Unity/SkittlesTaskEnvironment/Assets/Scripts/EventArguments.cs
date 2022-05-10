using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventArguments : MonoBehaviour
{
    public static EventArguments Instance { get; private set; }

    public Action OnSpacebar;
    public Action OnSpacebarDown;
    public Action OnH;
    public Action OnS;
    public Action OnMouseDown;
    public Action OnMouseUp;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) OnSpacebar?.Invoke();
        if (Input.GetKeyDown(KeyCode.Space)) OnSpacebarDown?.Invoke();
        if (Input.GetKey(KeyCode.H)) OnH?.Invoke();
        if (Input.GetKey(KeyCode.S)) OnS?.Invoke();
        if (Input.GetMouseButton(0)) OnMouseDown?.Invoke();
        if (!Input.GetMouseButton(0)) OnMouseUp?.Invoke();

    }
}
