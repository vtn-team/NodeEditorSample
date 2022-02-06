using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;
using Status = UnityEngine.UIElements.DropdownMenuAction.Status;

public class EventGraphToolbarView : ToolbarView
{
	public EventGraphToolbarView(BaseGraphView graphView) : base(graphView) { }

	protected override void AddButtons()
	{
		bool conditionalProcessorVisible = graphView.GetPinnedElementStatus<ConditionalProcessorView>() != Status.Hidden;
		AddToggle("フラグ確認", conditionalProcessorVisible, (v) => graphView.ToggleView<ConditionalProcessorView>());
		AddButton("保存", () => { (graphView as EventGraphView)?.Save(); });
	}
}