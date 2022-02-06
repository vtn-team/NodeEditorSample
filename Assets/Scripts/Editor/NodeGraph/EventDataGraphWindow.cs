using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;

public class EventDataGraphWindow : BaseGraphWindow
{
	BaseGraph tmpGraph;
	GameEventNodeView.EventNodeCallback _callback;

	public static BaseGraphWindow OpenFromDataNode(BaseGraph graph, GameEventNodeView.EventNodeCallback callback)
	{
		var window = EditorWindow.GetWindow<EventDataGraphWindow>();
		window.InitializeGraph(graph);
		window._callback = callback;
		return window;
	}

	protected override void OnDestroy()
	{
		var ev = graph as EventData;
		ev.Save();
		_callback?.Invoke(GameEventNodeView.EventNodeEventType.WindowDestroy, null);
		graphView?.Dispose();
	}

    private void OnProjectChange()
	{
		_callback?.Invoke(GameEventNodeView.EventNodeEventType.AssetChanged, null);
		//var ev = graph as EventData;
		//ev.Save();
	}

    protected override void InitializeWindow(BaseGraph graph)
	{
		titleContent = new GUIContent("イベントデータツリー");

		if (graphView == null)
		{
			graphView = new EventDataGraphView(this);
		}
		rootView.Add(graphView);
	}

	protected override void InitializeGraphView(BaseGraphView view)
	{
		// graphView.OpenPinned< ExposedParameterView >();
		// toolbarView.UpdateButtonStatus();
	}
}
