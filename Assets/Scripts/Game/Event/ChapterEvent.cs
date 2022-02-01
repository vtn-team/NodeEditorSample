using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using GraphProcessor;
using UnityEngine;

[CreateAssetMenu(fileName = "ChapterEvent", menuName = "ChapterEvent", order = 1)]
public class ChapterEvent :ScriptableObject
{
    [SerializeField] List<GameEvent> _eventList;
    public List<GameEvent> EventList => _eventList;
    

    public void ClearAllEventParams()
    {
        _eventList.ForEach(e => e.ClearTempValue());
    }

    public List<GameEvent> GetRunnableList()
    {
        return _eventList.Where(e => e.CheckRun()).ToList();
    }
}