using GraphProcessor;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[CreateAssetMenu(menuName = "Chapter Graph")]
public class Chapter : BaseGraph
{
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
