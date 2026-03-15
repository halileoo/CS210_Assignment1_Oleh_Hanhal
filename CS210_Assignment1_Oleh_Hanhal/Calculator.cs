using System.Collections;

namespace CS210_Assignment1_Oleh_Hanhal;

public class Calculator
{
    private CustomDict _variables = new CustomDict();
    
    private int GetPriority(string oper)
    {
        switch (oper)
        {
            case "(":
                return 0;   
            
            case "+":
            case "-":
                return 1;
            
            case "*":
            case "/":
                return 2;
            
            case "^":
            case "sin":
            case "cos":
            case "max":    
                return 3;
            
            default:
                return -1;
        }
    }
    
    public CustomArrayList Tokenize(string input)
    {   
        CustomArrayList tokens = new CustomArrayList();
        string buffer = "";
        string var_buffer = "";

        if (!input.Contains('='))
        {
            for (int i = 0; i < input.Length; i++)
            {
                var s = input[i];
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
                    
                    if (var_buffer != "")
                    {
                        if (_variables.ContainsKey(var_buffer.ToString()))
                        {
                            var var_tokenized = Tokenize(_variables.GetValue(var_buffer.ToString()));
                            for (int p = 0; p < var_tokenized.Count(); p++)
                                tokens.Add(var_tokenized.GetAt(p));
                        }
                        var_buffer = "";
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
                    
                    if (var_buffer != "")
                    {
                        if (_variables.ContainsKey(var_buffer.ToString()))
                        {
                            var var_tokenized = Tokenize(_variables.GetValue(var_buffer.ToString()));
                        
                            for (int p = 0; p < var_tokenized.Count(); p++)
                            {
                                tokens.Add(var_tokenized.GetAt(p));
                            }
                        }

                        var_buffer = "";
                    }
                }
            
                else if (s == 's' && (i+2) < input.Length)
                {
                    if (input[i+1] == 'i' && input[i+2] == 'n')
                        tokens.Add("sin");
                    i += 2;
                }
            
                else if (s == 'c' && (i+2) < input.Length)
                {
                    if (input[i+1] == 'o' && input[i+2] == 's')
                        tokens.Add("cos");
                    i += 2;
                }
                
                else if (s == 'm' && (i+2) < input.Length)
                {
                    if (input[i+1] == 'a' && input[i+2] == 'x')
                        tokens.Add("max");
                    i += 2;
                }

                else
                {
                    var_buffer += s.ToString();
                }
            }
        }
        
        else
        {
            var variable_name = "";
            var equal_index = input.IndexOf('=');
            _variables.AddItem((input[..equal_index].Replace(" ", ""),
                                input[(equal_index + 1)..]));
        }
        
        if (buffer != "")
        {
            tokens.Add(buffer);
        }
        
        if (var_buffer != "")
        {
            if (_variables.ContainsKey(var_buffer.ToString()))
            {
                var var_tokenized = Tokenize(_variables.GetValue(var_buffer.ToString()));
                for (int p = 0; p < var_tokenized.Count(); p++)
                    tokens.Add(var_tokenized.GetAt(p));
            }
            var_buffer = "";
        }
        
        return tokens;
    }
    
    public CustomArrayList TurnToRPN(CustomArrayList tokens)
    {
        CustomStack operatorStack = new CustomStack();
        CustomArrayList result = new CustomArrayList();

        for (var i = 0; i < tokens.Count(); i++)
        {
            string token = tokens.GetAt(i);
            if (double.TryParse(token, out double token_parsed ))
            {
                result.Add(token);
            }

            else if (
                token == "+" ||
                token == "-" ||
                token == "*" ||
                token == "/" ||
                token == "^" ||
                token == "sin" ||
                token == "cos" ||
                token == "max"
                )
            {
                while (
                    operatorStack.Peek() != null &&
                    operatorStack.Peek() != "(" &&
                    (
                        GetPriority(token) < GetPriority(operatorStack.Peek()) ||
                        (GetPriority(token) == GetPriority(operatorStack.Peek()) && token != "^")
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
                double result = num2 + num1;
                
                s.Push(result.ToString());
            }
            
            else if (token == "-")
            {
                double num1 = double.Parse(s.Pop());
                double num2 = double.Parse(s.Pop());
                double result = num2 - num1;
                
                s.Push(result.ToString());
            }
            
            else if (token == "*")
            {
                double num1 = double.Parse(s.Pop());
                double num2 = double.Parse(s.Pop());
                double result = num2 * num1;
                
                s.Push(result.ToString());
            }
            
            else if (token == "/")
            {
                double num1 = double.Parse(s.Pop());
                double num2 = double.Parse(s.Pop());
                double result = num2 / num1;
                
                s.Push(result.ToString());
            }
            
            else if (token == "^")
            {
                double num1 = double.Parse(s.Pop());
                double num2 = double.Parse(s.Pop());
                double result = Math.Pow(num2, num1);
                
                s.Push(result.ToString());
            }
            
            else if (token == "sin")
            {
                double num = double.Parse(s.Pop());
                var result = Math.Sin(num); 
                s.Push(result.ToString());
            }
            
            else if (token == "cos")
            {
                double num = double.Parse(s.Pop());
                var result = Math.Cos(num); 
                s.Push(result.ToString());
            }
            
            else if (token == "max")
            {
                double num1 = double.Parse(s.Pop());
                double num2 = double.Parse(s.Pop());
                double result = Math.Max(num2, num1);
                
                s.Push(result.ToString());
            }
        }
        
        return double.Parse(s.Pop());
    }

    public void Compute()
    {
        while (true)
        {
            Console.WriteLine("to exit type \"exit\" ");
            Console.Write(">>> ");
            string input = Console.ReadLine();

            if (input == "exit")
                break;
            
            CustomArrayList tokens = Tokenize(input);
            
            if (tokens.Count() > 0)
            {
                CustomArrayList rpn = TurnToRPN(tokens);
                Console.WriteLine(Calculate(rpn));
            }
        }
    }   
}