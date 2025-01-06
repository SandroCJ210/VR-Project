using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMemoryState", menuName = "MeowAI/Memory State")]
public class MemoryState : ScriptableObject
{
    [SerializeField] private Dictionary<string, object> _memory = new();
    public void SetData(string key, object value)
    {
        if (_memory.ContainsKey(key))
        {
            _memory[key] = value;
        }
        else
        {
            _memory.Add(key, value);
        }
    }

    public T GetData<T>(string key)
    {
        if (_memory.TryGetValue(key, out var value))
        {
            return (T)value;
        }
        return default;
    }

    public void RemoveData(string key)
    {
        if (_memory.ContainsKey(key))
        {
            _memory.Remove(key);
        }
    }

    public bool HasData(string key)
    {
        return _memory.ContainsKey(key);
    }

    public Dictionary<string, object> GetAllData()
    {
        return _memory;
    }
}
