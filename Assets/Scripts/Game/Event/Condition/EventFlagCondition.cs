using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class EventFlagCondition : IEventConditions
{
    public enum FlagCheck
    {
        Always,
        FlagOn,
        Equal,
        Greater,
        Less
    }

    [SerializeField] string _flagName;
    [SerializeField] int _flagParam;
    [SerializeField] FlagCheck _flagCheckMethod = FlagCheck.FlagOn;

    public string FlagName => _flagName;

    public bool Check()
    {
        int param = 0;//todo
        switch (_flagCheckMethod)
        {
            case FlagCheck.Always: return true;
            case FlagCheck.FlagOn: return (param > 0);
            case FlagCheck.Equal: return (param == _flagParam);
            case FlagCheck.Greater: return (param > _flagParam);
            case FlagCheck.Less: return (param < _flagParam);
            default:
                return false;
        }
    }
}
