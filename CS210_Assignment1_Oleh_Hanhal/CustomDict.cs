namespace CS210_Assignment1_Oleh_Hanhal;

public class CustomDict
{
    private (string key, string value)[] _array = new (string key, string value)[10];
    private int _pointer = 0;

    public string GetValue(string key)
    {
        if (ContainsKey(key))
        {
            for (var i = 0; i < _array.Length; i++)
            {
                if (_array[i].key == key)
                {
                    return _array[i].value;
                }
            }
        }
        
        else
        {
            Console.WriteLine($"no key {key}");
        }

        return "-1";
    }

    public void AddItem((string key, string value) item)
    {
        _array[_pointer] = item;
        _pointer += 1;
    }

    public bool ContainsKey(string key)
    {
        for (var i = 0; i < _array.Length; i++)
        {
            if (_array[i].key == key)
            {
                return true;
            }
        }

        return false;
    }

}