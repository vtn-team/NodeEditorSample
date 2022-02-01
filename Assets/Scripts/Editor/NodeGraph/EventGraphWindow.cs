using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;

public class EventGraphWindow : BaseGraphWindow
{
	BaseGraph tmpGraph;
	EventGraphToolbarView toolbarView;
	
	[MenuItem("Window/EventGraph")]
	public static BaseGraphWindow OpenWithTmpGraph()
	{
		var graphWindow = CreateWindow<EventGraphWindow>();

		// When the graph is opened from the window, we don't save the graph to disk
		graphWindow.tmpGraph = ScriptableObject.CreateInstance<BaseGraph>();
		graphWindow.tmpGraph.hideFlags = HideFlags.HideAndDontSave;
		graphWindow.InitializeGraph(graphWindow.tmpGraph);

		graphWindow.Show();

		return graphWindow;
	}

	protected override void OnDestroy()
	{
		graphView?.Dispose();
		DestroyImmediate(tmpGraph);
	}

	protected override void InitializeWindow(BaseGraph graph)
	{
		titleContent = new GUIContent("イベントツリー");

		if (graphView == null)
		{
			graphView = new EventGraphView(this);
			toolbarView = new EventGraphToolbarView(graphView);
			graphView.Add(toolbarView);
		}

		rootView.Add(graphView);
	}

	static Chapter _currentChapter;
	static public Chapter CurrentChapter => _currentChapter;

    protected override void OnEnable()
    {
		_currentChapter = graph as Chapter;
		base.OnEnable();
    }

    protected override void InitializeGraphView(BaseGraphView view)
	{
		// graphView.OpenPinned< ExposedParameterView >();
		// toolbarView.UpdateButtonStatus();
	}
}
