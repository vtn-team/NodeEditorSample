using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvent", order = 1)]
public class GameEvent : ScriptableObject
{
    [SerializeField] string _id;
    [SerializeReference, SubclassSelector]
    [SerializeField] List<IEventConditions> _condition = new List<IEventConditions>();
    [SerializeReference, SubclassSelector]
    [SerializeField] List<IEventData> _data;

    public List<IEventConditions> Conditions => _condition;

    bool _isEnter = false;
    bool _isExit = false;

    int _lastIndex = 0;

    public void ClearTempValue()
    {
        _lastIndex = 0;
        _isEnter = false;
        _isExit = false;
    }

    public bool CheckRun()
    {
        return _condition.All(c => c.Check());
    }

    public EventResultCode Run()
    {
        if (!_isEnter)
        {
            _data.ForEach(d => d.EnterEvent());
            _isEnter = true;
        }

        EventResultCode result = EventResultCode.Success;
        for (var i = _lastIndex; i < _data.Count; ++i)
        {
            var d = _data[i];
            result = d.Run();
            if(result != EventResultCode.Success)
            {
                break;
            }
        }

        if (result == EventResultCode.Success)
        {
            if (!_isExit)
            {
                _data.ForEach(d => d.ExitEvent());
                _isExit = true;
            }
        }

        return result;
    }

#if UNITY_EDITOR
    public string EventName => _id;
    public List<IEventData> Data => _data;
    public void SetCondition(List<IEventConditions> conds)
    {
        _condition = conds;
    }
    public void SetData(List<IEventData> data)
    {
        _data = data;
    }
#endif
}
