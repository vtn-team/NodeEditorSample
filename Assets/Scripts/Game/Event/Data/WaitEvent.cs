using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class WaitEvent : IEventData
{
    [SerializeField] float _waitTime;

    float _timer = 0;

    public void EnterEvent()
    {
        //
    }

    public EventResultCode Run()
    {
        _timer += Time.deltaTime;
        if (_timer > _waitTime)
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
