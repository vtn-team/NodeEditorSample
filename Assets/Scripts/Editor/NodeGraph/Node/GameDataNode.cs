using GraphProcessor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class GameDataNode : BaseNode
{
    // 入力ポートを定義
    [Input(allowMultiple = false)]
    public string input;

    // 出力ポートを定義
    [Output(allowMultiple = false)]
    public string output;

    [SerializeReference]
    IEventData _evtData;

    public IEventData Data => _evtData;

    public override string name => _evtData != null ? _evtData.GetType().ToString() : "";

    public void SetData(IEventData d)
    {
        _evtData = d;
    }

    // 計算処理
    protected override void Process()
    {
        output = input;
    }
}