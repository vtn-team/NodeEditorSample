using GraphProcessor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class GameConditionNode : BaseNode, IConditionNode
{
    // 出力ポートを定義
    [Output(allowMultiple = false)]
    public string output;

    [SerializeReference]
    IEventConditions _condition;

    public IEventConditions Condition => _condition;

    public override string name => _condition != null ? _condition.GetType().ToString() : "";

    public void SetCondition(IEventConditions d)
    {
        _condition = d;
    }

    protected override void Process()
    {
        //tbd
    }
}