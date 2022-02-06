using UnityEditor.UIElements;
using UnityEngine.UIElements;
using GraphProcessor;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.IO;

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
    TextField _title;

    protected override bool hasSettings => true;

    public override void OnCreated()
    {
        base.OnCreated();
    }

    protected void BuildContainer()
    {
        var node = nodeTarget as GameEventNode;

        dataContainer.Clear();

        if (node.Event == null)
        {
            _title = new TextField("イベント名");
            _title.style.minWidth = 300;
            _title.value = "";
            _title.maxLength = 99;
            _title.multiline = false;
            dataContainer.Add(_title);

            Button btn = new Button(NewData) { text = "新規データを作成" };
            dataContainer.Add(btn);

            dataContainer.Add(new Label() { text = "既存データを参照" });
            ObjectField objField = new ObjectField()
            {
                objectType = typeof(GameEvent),
                value = node.Event
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
            label.style.minWidth = 200;
            label.style.backgroundColor = new StyleColor(Color.gray);
            dataContainer.Add(label);

            foreach (var ev in node.Event.Data)
            {
                var evn = new Label() { text = ev.GetType().ToString() };
                evn.style.unityTextAlign = TextAnchor.MiddleCenter;
                evn.style.marginLeft = evn.style.marginRight = 10;
                evn.style.marginTop = evn.style.marginBottom = 2;
                evn.style.paddingLeft = evn.style.paddingRight = 10;
                evn.style.paddingTop = evn.style.paddingBottom = 5;
                evn.style.borderBottomColor = evn.style.borderTopColor = evn.style.borderRightColor = evn.style.borderLeftColor = new StyleColor(Color.gray);
                evn.style.borderBottomWidth = evn.style.borderTopWidth = evn.style.borderRightWidth = evn.style.borderLeftWidth = 2;
                evn.style.borderBottomLeftRadius = evn.style.borderBottomRightRadius = evn.style.borderTopLeftRadius = evn.style.borderTopRightRadius = 15;
                dataContainer.Add(evn);
            }

            Label evNum = new Label() { text = String.Format("イベント数:{0}", node.Event.Data.Count) };
            evNum.style.unityTextAlign = TextAnchor.MiddleCenter;
            evNum.style.backgroundColor = new StyleColor(Color.white);
            evNum.style.color = new StyleColor(Color.black);
            dataContainer.Add(evNum);

            /*
            //プロパティを表示する方
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
            */

            dataContainer.Add(new Button(Edit) { text = "編集" });

        }
    }

    protected override VisualElement CreateSettingsView()
    {
        var settings = new VisualElement();

        Button btn = new Button(Revoke) { text = "イベント関連付けを解除" };
        btn.style.color = new StyleColor(Color.red);
        settings.Add(btn);

        return settings;
    }

    public override void Enable()
    {
        RegisterCallback<MouseDownEvent>(OnMouseDown);
        BuildContainer();
    }

    void OnMouseDown(MouseDownEvent evt)
    {
        if(Time.realtimeSinceStartup - _lastClickTick < 0.2f)
        {
            Debug.Log("double click!!");
            Edit();
        }
        _lastClickTick = Time.realtimeSinceStartup;
    }

    void NewData()
    {
        if (_title.value == "") return;

        Chapter ch = EventGraphWindow.CurrentChapter;
        GameEvent evt = ScriptableObject.CreateInstance<GameEvent>();
        Debug.Log(String.Format("{0}/{1}/{2}.asset", EventGraphConst.EventAssetPath, ch.name, _title.value));

        string dir = String.Format("{0}/{1}", EventGraphConst.EventAssetPath, ch.name);
        string file = String.Format("{0}/{1}/{2}.asset", EventGraphConst.EventAssetPath, ch.name, _title.value);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        if(File.Exists(file))
        {
            Debug.LogError("file already exists. :" + file);
            return;
        }

        AssetDatabase.CreateAsset(evt, file);

        var node = nodeTarget as GameEventNode;
        node.RefreshNode(evt);
        node.LoadDataGraph();
        BuildContainer();
    }

    void Revoke()
    {
        var node = nodeTarget as GameEventNode;
        node.RefreshNode(null);
        BuildContainer();
    }

    void Edit()
    {
        var node = nodeTarget as GameEventNode;
        node.Graph.SyncData(node.Event);
        EventDataGraphWindow.OpenFromDataNode(node.Graph, EventCall);
    }

    void EventCall(EventNodeEventType evt, object data)
    {
        var node = nodeTarget as GameEventNode;

        switch(evt)
        {
            case EventNodeEventType.WindowDestroy:
                BuildContainer();
                break;
        }
    }
}