using UnityEditor.UIElements;
using UnityEngine.UIElements;
using GraphProcessor;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[NodeCustomEditor(typeof(GameEventNode))]
public class GameEventNodeView : BaseNodeView
{
    public enum EventNodeEventType
    {
        AssetChanged,
        SaveButton,
        WindowDestroy,
    }
    public delegate void EventNodeCallback(EventNodeEventType evt, object data);
    SerializedObject _target;
    float _lastClickTick = 0;

    public override void OnCreated()
    {
        base.OnCreated();
    }

    protected void BuildContainer()
    {
        var node = nodeTarget as GameEventNode;

        dataContainer.Clear();

        if (node._evt == null)
        {
            dataContainer.Add(new Label() { text = "参照データ" });
            ObjectField objField = new ObjectField()
            {
                objectType = typeof(GameEvent),
                value = node._evt
            };
            objField.RegisterValueChangedCallback(v => {
                node.RefreshNode(objField.value as GameEvent);
                node.LoadDataGraph();
                BuildContainer();
            });
            dataContainer.Add(objField);
        }
        else
        {
            node.LoadDataGraph();

            Label label = new Label() { text = "イベント内容" };
            label.style.backgroundColor = new StyleColor(Color.gray);
            dataContainer.Add(label);

            _target = new UnityEditor.SerializedObject(node._evt);
            dataContainer.Bind(_target);

            var dataProp = new PropertyField(_target.FindProperty("_data"), "Data");
            dataProp.style.minWidth = 300;
            dataProp.style.marginTop = 0;
            dataProp.style.marginBottom = 10;
            dataProp.style.marginLeft = 10;
            dataProp.style.marginRight = 10;
            dataProp.RegisterValueChangeCallback(d =>
            {
                node._evt.SetData(dataProp.userData as List<IEventData>);
            });
            dataContainer.Add(dataProp);

            Button btn = new Button(Revoke) { text = "イベント関連付けを解除" };
            btn.style.color = new StyleColor(Color.red);
            dataContainer.Add(btn);
        }

        //contentContainer.MarkDirtyRepaint();
        //dataContainer.MarkDirtyRepaint();
    }

    public override void Enable()
    {
        RegisterCallback<MouseDownEvent>(OnMouseDown);

        var node = nodeTarget as GameEventNode;

        TextField strField = new TextField
        {
            value = node.input
        };

        node.onProcessed += () => strField.value = node.input;

        strField.RegisterValueChangedCallback((v) => {
            owner.RegisterCompleteObjectUndo("Updated input");
            node.input = v.newValue;
        });

        BuildContainer();
    }

    void OnMouseDown(MouseDownEvent evt)
    {
        if(Time.realtimeSinceStartup - _lastClickTick < 0.2f)
        {
            Debug.Log("double click!!");
            //EventDataGraphWindow.OpenWithTmpGraph();
            var node = nodeTarget as GameEventNode;
            node._graph.SyncData(node._evt);
            var graph = EventDataGraphWindow.OpenFromDataNode(node._graph, EventCall);
        }
        _lastClickTick = Time.realtimeSinceStartup;
    }

    void Revoke()
    {
        var node = nodeTarget as GameEventNode;
        node._evt = null;
        Enable();
    }

    void EventCall(EventNodeEventType evt, object data)
    {
        var node = nodeTarget as GameEventNode;
    }
}