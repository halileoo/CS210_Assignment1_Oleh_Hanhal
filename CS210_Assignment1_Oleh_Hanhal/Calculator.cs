using System.Collections;

namespace CS210_Assignment1_Oleh_Hanhal;

public class Calculator
{
    public CustomArrayList Tokenize(string input)
    {
        CustomArrayList tokens = new CustomArrayList();
        string buffer = "";

        foreach (var s in input)
        {
            if (char.IsDigit(s) || s == '.' || s == ',')
            {
                buffer += s.ToString();
            }
            
            else if (s == '+' || s == '-' || s == '*' || s == '/' || s == '(' || s == ')' || s == '^')
            {
                if (buffer != "")
                {
                    tokens.Add(buffer);
                    buffer = "";
                }
                tokens.Add(s.ToString());
            }
            
            else if (s == ' ')
            {
                if (buffer != "")
                {
                    tokens.Add(buffer);
                    buffer = "";
                }
            }
        }
        
        if (buffer != "")
        {
            tokens.Add(buffer);
        }

        return tokens;
    }    
    
    public CustomArrayList TurnToRPN(CustomArrayList tokens)
    {
        Dictionary<string, int> priorities = new Dictionary<string, int>
        {
            { "(", 0 },
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 },
            { "^", 3 }
        };

        CustomStack operatorStack = new CustomStack();
        CustomArrayList result = new CustomArrayList();

        for (var i = 0; i < tokens.Count(); i++)
        {
            string token = tokens.GetAt(i);
            if (double.TryParse(token, out double token_parsed ))
            {
                result.Add(token);
            }

            else if (token == "+" || token == "-" || token == "*" || token == "/" || token == "^")
            {
                while (
                    operatorStack.Peek() != null &&
                    operatorStack.Peek() != "(" &&
                    (
                        priorities[token] < priorities[operatorStack.Peek()] ||
                        (priorities[token] == priorities[operatorStack.Peek()] && token != "^")
                    )
                ) 
                {
                    result.Add(operatorStack.Pop());
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
                        result.Add(operatorStack.Pop());
                }

                operatorStack.Pop();
            }
        }

        while (operatorStack.Peek() != null)
        {
            result.Add(operatorStack.Pop());
        }

        return result;
    }

    public double Calculate(CustomArrayList RPN)
    {
        CustomStack s = new CustomStack();

        for (var i = 0; i < RPN.Count(); i++)
        {
            var token = RPN.GetAt(i);

            if (double.TryParse(token, out double number))
                s.Push(token);
            
            else if (token == "+")
            {
                double num1 = double.Parse(s.Pop());
                double num2 = double.Parse(s.Pop());
                double num3 = num2 + num1;
                
                s.Push(num3.ToString());
            }
            
            else if (token == "-")
            {
                double num1 = double.Parse(s.Pop());
                double num2 = double.Parse(s.Pop());
                double num3 = num2 - num1;
                
                s.Push(num3.ToString());
            }
            
            else if (token == "*")
            {
                double num1 = double.Parse(s.Pop());
                double num2 = double.Parse(s.Pop());
                double num3 = num2 * num1;
                
                s.Push(num3.ToString());
            }
            
            else if (token == "/")
            {
                double num1 = double.Parse(s.Pop());
                double num2 = double.Parse(s.Pop());
                double num3 = num2 / num1;
                
                s.Push(num3.ToString());
            }
            
            else if (token == "^")
            {
                double num1 = double.Parse(s.Pop());
                double num2 = double.Parse(s.Pop());
                double num3 = Math.Pow(num2, num1);
                
                s.Push(num3.ToString());
            }
        }

        return double.Parse(s.Pop());
    }

    public double Compute(string input) => Calculate(TurnToRPN(Tokenize(input)));
}