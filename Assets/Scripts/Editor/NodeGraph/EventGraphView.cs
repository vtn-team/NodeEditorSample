using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using GraphProcessor;
using System;
using UnityEditor;
using System.Linq;

public class EventGraphView : BaseGraphView
{
	// Nothing special to add for now
	public EventGraphView(EditorWindow window) : base(window) { }

	public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
	{
		evt.menu.AppendSeparator();

		var mousePos = (evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
		Vector2 nodePosition = mousePos;

		evt.menu.AppendAction("イベントノード作成",
			(e) => CreateEventNode(nodePosition),
			DropdownMenuAction.AlwaysEnabled
		);

		//オーバヘッド結構あったらキャッシュする
		Type[] inheritedTypes = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(p => typeof(IEventConditions).IsAssignableFrom(p) && p.IsClass)
			.Prepend(null)
			.ToArray();

		var typeData = inheritedTypes.Select(type => type == null ? new Tuple<Type, string>(null, "<null>") : new Tuple<Type, string>(type, type.ToString()));

		foreach (var type in typeData)
		{
			if (type.Item1 == null) continue;

			evt.menu.AppendAction("条件ノード作成/" + type.Item2,
				(e) => CreateNodeOfType(type.Item1, nodePosition),
				DropdownMenuAction.AlwaysEnabled
			);
		}

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
	protected void CreateEventNode(Vector2 position)
	{
		RegisterCompleteObjectUndo("Added GameEventNode");
		AddNode(BaseNode.CreateFromType(typeof(GameEventNode), position));
	}

	/// <summary>
	/// Add the New Stack entry to the context menu
	/// </summary>
	/// <param name="evt"></param>
	protected void CreateNodeOfType(Type type, Vector2 position)
	{
		RegisterCompleteObjectUndo("Added " + type + " GameConditionNode");
		var node = BaseNode.CreateFromType(typeof(GameConditionNode), position) as GameConditionNode;
		var obj = Activator.CreateInstance(type);
		node.SetCondition(obj as IEventConditions);

		AddNode(node);
	}

	public void Save()
	{
		var ev = graph as Chapter;
		foreach (var n in ev.nodes)
		{
			GameEventNode node = n as GameEventNode;
			if (node == null) continue;

			node.SaveCondition();
		}
	}
}