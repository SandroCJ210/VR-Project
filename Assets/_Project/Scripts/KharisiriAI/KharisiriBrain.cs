using System.Collections.Generic;

public class KharisiriBrain
{
    private Dictionary<string, object> _memory = new();
    public void SetData(string key, object value)
    {
        _memory[key] = value;
    }

    public T GetData<T>(string key)
    {
        if (_memory.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default;
    }

    public bool RemoveData(string key)
    {
       return _memory.Remove(key);
    }

    public bool HasData(string key)
    {
        return _memory.ContainsKey(key);
    }

    public Dictionary<string, object> GetAllData()
    {
        return new Dictionary<string, object>(_memory);
    }
}