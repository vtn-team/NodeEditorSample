using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using GraphProcessor;
using System;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class EventDataGraphView : BaseGraphView
{
	// Nothing special to add for now
	public EventDataGraphView(EditorWindow window) : base(window) { }

	public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
	{
		evt.menu.AppendSeparator();

		var mousePos = (evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
		Vector2 nodePosition = mousePos;

		//オーバヘッド結構あったらキャッシュする
		Type[] inheritedTypes = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(p => typeof(IEventData).IsAssignableFrom(p) && p.IsClass)
			.Prepend(null)
			.ToArray();

		var typeData = inheritedTypes.Select(type => type == null ? new Tuple<Type, string>(null, "<null>") : new Tuple<Type, string>(type, type.ToString()));

		foreach (var type in typeData)
		{
			if (type.Item1 == null) continue;

			evt.menu.AppendAction("イベント作成/" + type.Item2,
				(e) => CreateNodeOfType(type.Item1, nodePosition),
				DropdownMenuAction.AlwaysEnabled
			);
		}
	}

	/// <summary>
	/// Add the New Stack entry to the context menu
	/// </summary>
	/// <param name="evt"></param>
	protected void CreateNodeOfType(Type type, Vector2 position)
	{
		RegisterCompleteObjectUndo("Added " + type + " node");
		var node = BaseNode.CreateFromType(typeof(GameDataNode), position) as GameDataNode;
		var obj = Activator.CreateInstance(type);
		node.SetData(obj as IEventData);

		AddNode(node);
	}
}