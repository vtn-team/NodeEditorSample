using UnityEditor.UIElements;
using UnityEngine.UIElements;
using GraphProcessor;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[NodeCustomEditor(typeof(GameDataNode))]
public class GameDataNodeView : BaseNodeView
{
    [Serializable]
    class DataBinder : ScriptableObject
    {
        [SerializeReference]
        public IEventData Data;
    }

    SerializedObject _target;
    DataBinder _binder = null;

    public override void Enable()
    {
        //RegisterCallback<MouseDownEvent>(OnMouseDown);
        var node = nodeTarget as GameDataNode;
        if(_binder == null)
        {
            _binder = ScriptableObject.CreateInstance<DataBinder>();
        }
        Type type = node.Data.GetType();
        _binder.Data = node.Data;
        _target = new UnityEditor.SerializedObject(_binder);
        dataContainer.Bind(_target);

        Label label = new Label() { text = "イベント内容" };
        label.style.backgroundColor = new StyleColor(Color.gray);
        dataContainer.Add(label);

        const BindingFlags bindingAttr =
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.FlattenHierarchy |
                BindingFlags.Instance
            ;
        Debug.Log(type.Name);
        var fields = type.GetFields(bindingAttr);
        foreach (var f in fields)
        {
            var dp = new PropertyField(_target.FindProperty("Data."+f.Name), f.Name.Replace("_",""));
            //dp.userData = f.GetValue(node.Data);
            dp.style.minWidth = 300;
            //dp.style.marginTop = 0;
            //dp.style.marginBottom = 10;
            dp.style.marginLeft = 10;
            dp.style.marginRight = 10;
            {
                var field = f;
                dp.RegisterValueChangeCallback(d =>
                {
                    node.SetData(_binder.Data);
                    //node._evt.SetData(dataProp.userData as List<IEventData>);
                });
            }
            dataContainer.Add(dp);
        }

        /*
        var dataProp = new PropertyField(_target.FindProperty("Data"));
        dataProp.style.minWidth = 300;
        dataProp.style.marginTop = 0;
        dataProp.style.marginBottom = 10;
        dataProp.style.marginLeft = 10;
        dataProp.style.marginRight = 10;
        dataProp.RegisterValueChangeCallback(d => {
            node.SetData(_binder.Data);
        });
        contentContainer.Add(dataProp);
        */
    }

    void OnMouseDown(MouseDownEvent evt)
    {
    }
}