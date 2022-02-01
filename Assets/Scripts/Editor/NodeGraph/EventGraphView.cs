using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using GraphProcessor;
using System;
using UnityEditor;

public class EventGraphView : BaseGraphView
{
	// Nothing special to add for now
	public EventGraphView(EditorWindow window) : base(window) { }

	public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
	{
		evt.menu.AppendSeparator();

		var mousePos = (evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
		Vector2 nodePosition = mousePos;

		/*
		foreach (var nodeMenuItem in NodeProvider.GetNodeMenuEntries())
		{
			evt.menu.AppendAction("イベント作成/" + nodeMenuItem.path,
				(e) => CreateNodeOfType(nodeMenuItem.type, nodePosition),
				DropdownMenuAction.AlwaysEnabled
			);
		}
		*/

		evt.menu.AppendAction("イベント作成",
			(e) => CreateNodeOfType(typeof(GameEventNode), nodePosition),
			DropdownMenuAction.AlwaysEnabled
		);

		evt.menu.AppendAction("グループ作成",
			(e) => AddStackNode(new BaseStackNode(nodePosition)),
			DropdownMenuAction.AlwaysEnabled
		);
		//base.BuildContextualMenu(evt);
	}

	/// <summary>
	/// Add the New Stack entry to the context menu
	/// </summary>
	/// <param name="evt"></param>
	protected void CreateNodeOfType(Type type, Vector2 position)
	{
		RegisterCompleteObjectUndo("Added " + type + " node");
		AddNode(BaseNode.CreateFromType(type, position));
	}
}