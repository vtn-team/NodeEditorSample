
using GraphProcessor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// イベントデータ。連携用なのでアセット保存はされない。
/// </summary>
public class EventData : BaseGraph
{
    GameEvent _target;
    public void SyncData(GameEvent evt)
    {
        _target = evt;

        BaseNode start;
        // StartNodeが無かったらつくる
        if (!nodes.Any(x => x is StartNode))
        {
            start = BaseNode.CreateFromType<StartNode>(Vector2.zero);
            AddNode(start);
        }
        else
        {
            start = nodes.Single(x => x is StartNode);
        }

        BaseNode parentNode = start;
        GameDataNode node = null;
        if (start.GetOutputNodes().Count() > 0)
        {
            var con = start.GetOutputNodes().ToList();
            node = con[0] as GameDataNode;
        }

        foreach (var d in evt.Data)
        {
            bool isCreate = false;
            if (node == null)
            {
                isCreate = true;
            }
            else
            {
                if (node.Data.GetType() == d.GetType())
                {
                    parentNode = node;
                    var con = node.GetOutputNodes().ToList();
                    if (con.Count > 0)
                    {
                        node = con[0] as GameDataNode;
                    }
                    continue;
                }
                isCreate = true;
            }

            if(isCreate)
            {
                var newNode = BaseNode.CreateFromType(typeof(GameDataNode), parentNode.position.position + new Vector2(400, 0)) as GameDataNode;
                newNode.SetData(d);
                AddNode(newNode);

                if (newNode.inputPorts.Count > 0 && parentNode.outputPorts.Count > 0)
                {
                    Connect(newNode.inputPorts[0], parentNode.outputPorts[0]);
                }
                else
                {
                    Debug.LogWarning("cant connect");
                    RemoveNode(newNode);
                    break;
                }

                node = newNode;
                parentNode = node;
            }
        }
    }

    public void Save()
    {
        if (_target == null) return;

        List<IEventData> data = new List<IEventData>();

        if (!nodes.Any(x => x is StartNode))
        {
            Debug.LogWarning("no start");
            return;
        }
        BaseNode start = nodes.Single(x => x is StartNode);
        GameDataNode node = null;
        if (start.GetOutputNodes().Count() == 0)
        {
            Debug.Log("no list");
            _target.SetData(data);
            return;
        }

        var con = start.GetOutputNodes().ToList();
        node = con[0] as GameDataNode;
        if (node == null)
        {
            Debug.Log("node is null");
            _target.SetData(data);
            return;
        }

        while(node != null)
        {
            data.Add(node.Data);

            var connect = node.GetOutputNodes().ToList();
            if (connect.Count > 0)
            {
                node = connect[0] as GameDataNode;
            }
            else
            {
                node = null;
            }
        }

        _target.SetData(data);
    }
}
