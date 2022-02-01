using GraphProcessor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
[NodeMenuItem("GameEvent")] // 作成時のメニュー名
public class GameEventNode : BaseNode
{
    // 入力ポートを定義
    [Input(name = "条件", allowMultiple = true)]
    public string input;

    // 出力ポートを定義
    [Output(name = "次", allowMultiple = true)]
    public string output;

    [SerializeField]
    private string _graphJson;

    public GameEvent _evt;
    [SerializeField] private string _graphGUID;
    public EventData _graph;

    public override bool isRenamable => true;
    public override string name => _evt ? _evt.EventName : "空のイベント";

    static string[] SearchPaths = { "/Assets/DataAsset/GraphAsset" };
    const string GraphAssetPath = "Assets/DataAsset/GraphAsset";

    public void RefreshNode(GameEvent evt)
    {
        _evt = evt;
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

        if (_graphGUID != guid)
        {
            _graph = null;
            _graph = AssetDatabase.LoadAssetAtPath<EventData>(String.Format("{0}/{1}.asset", GraphAssetPath, guid));
            Debug.Log(_graph);
            Debug.LogWarning("load asset");

            /*
            var files = AssetDatabase.FindAssets("t:EventData", SearchPaths);
            foreach(var f_guid in files)
            {
                if (guid != f_guid) continue;

                Debug.LogWarning("load asset");
                var path = AssetDatabase.GUIDToAssetPath(f_guid);
                _graph = AssetDatabase.LoadAssetAtPath<EventData>(path);
                _graphGUID = f_guid;
            }
            */
        }
        if (_graph == null)
        {
            Debug.LogWarning("create new asset");
            _graph = ScriptableObject.CreateInstance<EventData>();
            AssetDatabase.CreateAsset(_graph, String.Format("{0}/{1}.asset", GraphAssetPath, guid));
        }

        _graph.SyncData(_evt);
    }

    // 計算処理
    protected override void Process()
    {
        output = input;
    }
}