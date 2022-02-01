using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class TransitionEvent : IEventData
{
    public enum TransitionType
    {
        FadeIn,
        FadeOut,
    }

    [SerializeField] TransitionType _type;
    [SerializeField] bool _fallthrough = false;
    [SerializeField] float _transitionTime;

    float _timer = 0;
    bool _isEnd = false;

    public void EnterEvent()
    {
        //
    }

    public EventResultCode Run()
    {
        if (_fallthrough)
        {
            return EventResultCode.Success;
        }
        if (_isEnd)
        {
            return EventResultCode.Success;
        }
        return EventResultCode.Working;
    }

    public void ExitEvent()
    {
        //
    }
}
