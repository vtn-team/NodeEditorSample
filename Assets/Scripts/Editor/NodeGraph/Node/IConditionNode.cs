using UnityEditor;
using UnityEngine;

public interface IConditionNode
{
    IEventConditions Condition { get; }
}