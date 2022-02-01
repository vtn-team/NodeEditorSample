using GraphProcessor;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[CreateAssetMenu(menuName = "Chapter Graph")]
public class Chapter : BaseGraph
{
    /*
    public List<EventData> EventNodeList;

    public EventData GetEventData(string guid)
    {
        var node = EventNodeList.Where(n => n.NodeGUID == guid).ToList();
        if(node.Count == 0)
        {
            var obj = ScriptableObject.CreateInstance<EventData>();
            EventNodeList.Add(obj);
            return obj;
        }
        return node[0];
    }
    */

#if UNITY_EDITOR
    // ダブルクリックでウィンドウが開かれるように
    [OnOpenAsset(0)]
    public static bool OnBaseGraphOpened(int instanceID, int line)
    {
        var asset = EditorUtility.InstanceIDToObject(instanceID) as Chapter;
        if (asset == null) return false;

        var window = EditorWindow.GetWindow<EventGraphWindow>();
        window.InitializeGraph(asset);
        return true;
    }
#endif
}
