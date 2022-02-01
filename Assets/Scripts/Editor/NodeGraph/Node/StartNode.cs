using GraphProcessor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class StartNode : BaseNode
{
    // 出力ポートを定義
    [Output(name = "最初のイベント", allowMultiple = false)]
    public string output;

    public override bool deletable => false;

    // 計算処理
    protected override void Process()
    {
        output = "ok";
    }
}