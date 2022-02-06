using GraphProcessor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[Serializable]
[NodeMenuItem("GameEvent")] // 作成時のメニュー名
public class GameEventNode : BaseNode, IConditionNode
{
    // 入力ポートを定義
    [Input(name = "条件", allowMultiple = true)]
    public string input;

    // 出力ポートを定義
    [Output(name = "次", allowMultiple = true)]
    public string output;

    IEventConditions _condition;

    [SerializeField] GameEvent _evt;
    [SerializeField] private string _graphGUID;
    EventData _graph;

    public IEventConditions Condition => _condition;
    public GameEvent Event => _evt;
    public EventData Graph => _graph;

    public override bool isRenamable => true;
    public override string name => _evt ? _evt.EventName : "空のイベント";

    static string[] SearchPaths = { "/Assets/DataAsset/GraphAsset" };

    void CreateCondition()
    {
        //リフレクションで対応
        const BindingFlags bindingAttr =
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.FlattenHierarchy |
                BindingFlags.Instance;

        _condition = new EventFlagCondition();
        Type type = typeof(EventFlagCondition);
        var fields = type.GetFields(bindingAttr);
        foreach(var f in fields)
        {
            switch (f.Name)
            {
                case "_flagName":
                    f.SetValue(_condition, name);
                    break;
                case "_flagCheckMethod":
                    f.SetValue(_condition, EventFlagCondition.FlagCheck.FlagOn);
                    break;

                default:
                    break;
            }
        }
    }

    public void RefreshNode(GameEvent evt)
    {
        _evt = evt;
        if (_evt == null) return;

        CreateCondition();
    }

    public void LoadDataGraph()
    {
        string guid;
        long localId;

        if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(_evt, out guid, out localId))
        {
            Debug.LogWarning("cant get guid");
            return;
        }

        if (!Directory.Exists(EventGraphConst.GraphAssetPath))
        {
            Directory.CreateDirectory(EventGraphConst.GraphAssetPath);
        }

        if (_graphGUID != guid)
        {
            _graph = null;
            _graph = AssetDatabase.LoadAssetAtPath<EventData>(String.Format("{0}/{1}.asset", EventGraphConst.GraphAssetPath, guid));
            //Debug.LogWarning("load asset");
        }
        if (_graph == null)
        {
            Debug.Log("create new asset");
            _graph = ScriptableObject.CreateInstance<EventData>();
            AssetDatabase.CreateAsset(_graph, String.Format("{0}/{1}.asset", EventGraphConst.GraphAssetPath, guid));
        }

        CreateCondition();
        _graph.SyncData(_evt);
    }

    public void SaveCondition()
    {
        List<IEventConditions> conds = new List<IEventConditions>();
        foreach(var n in GetInputNodes())
        {
            var cond = n as IConditionNode;
            if (cond == null) continue;
            if (cond.Condition == null) continue;

            conds.Add(cond.Condition);
        }
        _evt.SetCondition(conds);
    }

    // 計算処理
    protected override void Process()
    {
        //tbd
    }
}