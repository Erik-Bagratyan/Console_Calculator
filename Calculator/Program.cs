using System;
using System.Text;
using System.Collections.Generic;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // ***************  infix   ****************
            Console.Write("Enter Line : ");
            StringBuilder text = new StringBuilder(Console.ReadLine());
            text.Replace(" ", "");
            Console.WriteLine($"infix : {text.ToString()}");

            string[] postfix = postfix_line(text.ToString());
            Console.Write("postfix : ");
            for(int i = 0; postfix[i] != "End"; ++i)
            {
                Console.Write(postfix[i] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Result : " + result(postfix));
            Console.ReadKey();

        }
        static double m_mul(double a, double b)
        {
	        return a * b;
        }

        static double m_div(double a, double b)
        {
            if(b == 0)
            {
                Console.WriteLine("ERROR : the number is divisible by 0");
                Environment.Exit(1);
            }
	        return (a / b);
        }

        static double m_add(double a, double b)
        {
	        return a + b;
        }

        static double m_sub(double a, double b)
        {
	        return a - b;
        }
     
        static double eval(char op, double a, double b)
        {
	        switch (op)
	        {
	        case '-': return m_sub(b, a);
	        case '+': return m_add(b, a);
	        case '/': return m_div(b, a);
	        case '*': return m_mul(b, a);
		        default:
			        Console.WriteLine("WARNINGUnknow operator =>" + a);
			        break;
	        }
            return 0;
        }

        static double result(string[] postfix)
        {
            Stack<double> stack_result = new Stack<double>();
            for (int i = 0; postfix[i] != "End"; ++i)
            {
                if (postfix[i] != "+" && postfix[i] != "-" && postfix[i] != "*" && postfix[i] != "/")
                    stack_result.Push(System.Convert.ToDouble(postfix[i]));
                else
                    stack_result.Push(eval(System.Convert.ToChar(postfix[i]), stack_result.Pop(), stack_result.Pop()));
            }
            return stack_result.Pop();
        }

        static string[] postfix_line(string infix)
        {
            int post_len = 0;
            string[] post_line = new string[infix.Length + 1];
            StringBuilder temp_number = new StringBuilder();
            Stack<char> stack_operators = new Stack<char>();
            for (int i = 0; i < infix.Length; ++i)
            {
                switch(infix[i])
                {
                    case '(': CaseToOpenBracket(stack_operators); break;
                    case ')': CaseToCloseBracket(post_line, stack_operators, ref post_len, temp_number); break;
                    case '+': CaseToAddORSub(post_line, stack_operators, infix[i], ref post_len, temp_number); break;
                    case '-': CaseToAddORSub(post_line, stack_operators, infix[i], ref post_len, temp_number); break;
                    case '*': CaseToMulORDiv(post_line, stack_operators, infix[i], ref post_len, temp_number); break;
                    case '/': CaseToMulORDiv(post_line, stack_operators, infix[i], ref post_len, temp_number); break;
                    default: ConcateDigit(temp_number, infix[i]); break;
                }
            }
            if (temp_number.Length != 0)
                post_line[post_len++] = temp_number.ToString();
            while (stack_operators.Count != 0)
            {
                post_line[post_len++] = stack_operators.Pop().ToString();
            }
            post_line[post_len] = "End";

            return post_line;
        }

        static void CaseToCloseBracket(string[] post_line, Stack<char> stack_operators, 
                                            ref int post_len, StringBuilder temp_number)
        {
            NumberConcatePostfix(post_line, temp_number, ref post_len);
            char temp;
            while (stack_operators.Count != 0 && (temp = stack_operators.Pop()) != '(')
                post_line[post_len++] = temp.ToString();
        }

        static void NumberConcatePostfix(string[] post_line, StringBuilder temp_number, ref int post_len)
        {
            if (temp_number.Length != 0)
            {
                post_line[post_len++] = temp_number.ToString();
                temp_number.Clear();
            }
        }

        static void CaseToOpenBracket(Stack<char> stack_operators)
        {
            stack_operators.Push('(');
        }

        static void CaseToMulORDiv(string[] post_line, Stack<char> stack_operators,
                                   char _oper, ref int post_len, StringBuilder temp_number)
        { 
            NumberConcatePostfix(post_line, temp_number, ref post_len);
            char temp;
            if (stack_operators.Count == 0 || ((temp = stack_operators.Peek()) == '+' || temp == '-' || temp == '('))
            {
                stack_operators.Push(_oper);
                return;
            }
            post_line[post_len++] = stack_operators.Pop().ToString();
            stack_operators.Push(_oper);
        }

        static void CaseToAddORSub(string[] post_line, Stack<char> stack_operators,
                                    char _oper, ref int post_len, StringBuilder temp_number)
        {
            NumberConcatePostfix(post_line, temp_number, ref post_len);
            char temp;
                while (stack_operators.Count != 0 && (temp = stack_operators.Peek()) != '(')
                    post_line[post_len++] = stack_operators.Pop().ToString();
                stack_operators.Push(_oper);
        }

        static void ConcateDigit(StringBuilder temp_number, char value)
        {
            temp_number.Append(value);
        }
    }
}
