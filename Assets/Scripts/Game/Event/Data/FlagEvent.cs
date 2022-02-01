using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class FlagEvent : IEventData
{
    public enum ActionType
    {
        FlagON,
        FlagSetValue,
        FlagOFF
    }

    [SerializeField] string _flagName;
    [SerializeField] int _value = 1;
    [SerializeField] ActionType _action = ActionType.FlagON;

    public void EnterEvent()
    {
        //
    }

    public EventResultCode Run()
    {
        if (_flagName == "") return EventResultCode.Error;

        //
        switch(_action)
        {
            case ActionType.FlagON:
                break;

            case ActionType.FlagSetValue:
                break;

            case ActionType.FlagOFF:
                break;
        }

        return EventResultCode.Success;
    }

    public void ExitEvent()
    {
        //
    }
}
