using UnityEditor.UIElements;
using UnityEngine.UIElements;
using GraphProcessor;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[NodeCustomEditor(typeof(GameConditionNode))]
public class GameConditionNodeView : BaseNodeView
{
    [Serializable]
    class DataBinder : ScriptableObject
    {
        [SerializeReference]
        public IEventConditions Condition;
    }

    SerializedObject _target;
    DataBinder _binder = null;

    public override void Enable()
    {
        titleContainer.style.backgroundColor = new StyleColor(new Color(64,64,64));

        //RegisterCallback<MouseDownEvent>(OnMouseDown);
        var node = nodeTarget as GameConditionNode;
        if (_binder == null)
        {
            _binder = ScriptableObject.CreateInstance<DataBinder>();
        }
        Type type = node.Condition.GetType();
        _binder.Condition = node.Condition;
        _target = new UnityEditor.SerializedObject(_binder);
        dataContainer.Bind(_target);

        Label label = new Label() { text = "イベント内容" };
        label.style.backgroundColor = new StyleColor(Color.gray);
        dataContainer.Add(label);

        const BindingFlags bindingAttr =
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.FlattenHierarchy |
                BindingFlags.Instance;

        var fields = type.GetFields(bindingAttr);
        foreach (var f in fields)
        {
            var dp = new PropertyField(_target.FindProperty("Condition." + f.Name), f.Name.Replace("_", ""));
            dp.style.minWidth = 300;
            dp.style.marginLeft = 10;
            dp.style.marginRight = 10;
            {
                var field = f;
                dp.RegisterValueChangeCallback(d =>
                {
                    node.SetCondition(_binder.Condition);
                    //node._evt.SetData(dataProp.userData as List<IEventData>);
                });
            }
            dataContainer.Add(dp);
        }
    }

    void OnMouseDown(MouseDownEvent evt)
    {
    }
}