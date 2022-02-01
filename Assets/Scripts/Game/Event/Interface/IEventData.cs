using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum EventResultCode
{
    None = -99,
    Error = -1,
    Success = 0,
    Pending = 1,
    Working = 2,
}

public interface IEventData
{
    void EnterEvent();
    EventResultCode Run();
    void ExitEvent();
}