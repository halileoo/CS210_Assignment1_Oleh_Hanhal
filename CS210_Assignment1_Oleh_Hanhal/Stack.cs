namespace CS210_Assignment1_Oleh_Hanhal;

public class CustomStack
{
    private const int Capacity = 50;

    private string[] _array = new string[Capacity];

    private int _pointer;

    public void Push(string value)
    {
        if (_pointer == _array.Length)
        {
            throw new Exception("Stack overflowed");
        }

        _array[_pointer] = value;
        _pointer++;
    }

    public string Pop()
    {
        if (_pointer == 0)
        {
            return null;
        }
        
        _pointer--;
        var value = _array[_pointer];
        return value;
    }

    public string Peek()
    {
        if (_pointer > 0)
        {
            var value = _array[_pointer - 1];   
            
            return value;
        }
        
        else
        {
            Console.WriteLine("Stack is empty");
        }

        return null;
    }
}