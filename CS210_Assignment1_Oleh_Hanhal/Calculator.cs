using System.Collections;

namespace CS210_Assignment1_Oleh_Hanhal;

public class Calculator
{
    private CustomArrayList _tokens = new CustomArrayList();
    
    public void Tokenize(string input)
    {
        string buffer = "";

        foreach (var s in input)
        {
            if (char.IsDigit(s))
            {
                buffer += s.ToString();
            }
            
            else if (s == '+' || s == '-' || s == '*' || s == '/' || s == '(' || s == ')')
            {
                if (buffer != "")
                {
                    _tokens.Add(buffer);
                    buffer = "";
                }
                _tokens.Add(s.ToString());
            }
            
            else if (s == ' ')
            {
                if (buffer != "")
                {
                    _tokens.Add(buffer);
                    buffer = "";
                }
            }
        }
        
        if (buffer != "")
        {
            _tokens.Add(buffer);
        }
    }    
    
    public string TurnToRPN()
    {
        Dictionary<string, int> priorities = new Dictionary<string, int>
        {
            { "(", 0 },
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 }
        };

        CustomStack operatorStack = new CustomStack();
        string result = "";

        for (var i = 0; i < _tokens.Count(); i++)
        {
            string token = _tokens.GetAt(i);
            if (int.TryParse(token, out int parsed_number))
            {
                result += token + " ";
            }

            else if (token == "+" || token == "-" || token == "*" || token == "/")
            {
                while (operatorStack.Peek() != null &&
                       operatorStack.Peek() != "(" &&
                       priorities[token] <= priorities[operatorStack.Peek()])
                {
                    result += operatorStack.Pop() + " ";
                }

                operatorStack.Push(token);
            }

            else if (token == "(")
            {
                operatorStack.Push(token);
            }

            else if (token == ")")
            {
                while (operatorStack.Peek() != null &&
                       operatorStack.Peek() != "(")
                {
                        result += operatorStack.Pop() + " ";
                }

                operatorStack.Pop();
            }
        }

        while (operatorStack.Peek() != null)
        {
            result += operatorStack.Pop() + " ";
        }

        return result.Trim();
    }
}