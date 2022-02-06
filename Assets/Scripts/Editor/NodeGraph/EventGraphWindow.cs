using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;

public class EventGraphWindow : BaseGraphWindow
{
	BaseGraph tmpGraph;
	EventGraphToolbarView toolbarView;

	protected override void OnDestroy()
	{
		graphView?.Dispose();
	}

	private void OnProjectChange()
	{
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
