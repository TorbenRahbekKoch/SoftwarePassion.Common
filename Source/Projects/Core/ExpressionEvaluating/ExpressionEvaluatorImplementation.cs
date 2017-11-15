using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SoftwarePassion.Common.Core.ExpressionEvaluating
{
    internal class ExpressionEvaluatorImplementation
    {
        public ExpressionEvaluatorImplementation(string expression, IEnumerable<bool> criteriaValues)
        {
            this.expression = expression.Trim();
            this.criteriaValues = new List<bool>(criteriaValues);
        }

        public bool Eval()
        {
            if (expression.Length == 0)
            {
                if (criteriaValues.Any(cv => cv == false))
                    return false;

                return true;
            }

            Reset();

            while (true)
            {
                if (!Next())
                    break;

                if (type == Type.Operand) // A B C etc.
                {
                    operandStack.Push(curOperand);
                }
                else // operators
                {
                    Operator previous = (operatorStack.Count == 0) ? Operator.ParenthesisBegin : operatorStack.Peek();
                    if (curOperator == Operator.ParenthesisEnd)
                    {
                        while (Do_npr()) ;
                    }
                    if (previous > curOperator)
                    {
                        Do_npr();
                        operatorStack.Push(curOperator);
                    }
                    else
                    {
                        operatorStack.Push(curOperator);
                    }
                }
            }

            while (Do_npr()) ;

            if (operandStack.Count == 0)
                return false; // bug in expression.
            return operandStack.Peek();
        }

        enum Type
        {
            Operand,
            Operator
        };

        enum Operator
        {
            ParenthesisEnd,
            And,
            Or,
            Xor,
            UnaryNot,
            ParenthesisBegin,
        };

        private bool Next()
        {
            // Skip whitespace:
            Match match = regexp.Match(expression, pos);
            if (!match.Success)
                return false;
            // Since match.Success obviously is true here match.Index will always have a sane value
            pos = match.Index;

            // Find where token ends:
            int newPos = pos + 1;
            if (expression[pos] != '(' && expression[pos] != ')')
            {
                Match match2 = regexp2.Match(expression, pos + 1);
                newPos = match2.Index;
                if (newPos <= pos || !match2.Success)
                    newPos = expression.Length;
            }

            token = expression.Substring(pos, newPos - pos);
            pos = newPos;

            if (token == "AND")
            {
                type = Type.Operator;
                curOperator = Operator.And;
            }
            else if (token == "OR")
            {
                type = Type.Operator;
                curOperator = Operator.Or;
            }
            else if (token == "XOR")
            {
                type = Type.Operator;
                curOperator = Operator.Xor;
            }
            else if (token == "NOT")
            {
                type = Type.Operator;
                curOperator = Operator.UnaryNot;
            }
            else if (token == "(")
            {
                type = Type.Operator;
                curOperator = Operator.ParenthesisBegin;
            }
            else if (token == ")")
            {
                type = Type.Operator;
                curOperator = Operator.ParenthesisEnd;
            }
            else if (token[0] >= 'A' && token[0] <= 'Z')
            {
                type = Type.Operand;
                //Encoding unicode = Encoding.Unicode;
                //byte[] unicodeBytes = unicode.GetBytes(token.Substring(0, 1));
                int valueIndex = token[0] - 65;
                if (valueIndex < criteriaValues.Count() && valueIndex >= 0)
                    curOperand = criteriaValues[valueIndex];
                else
                    throw new Exception("More operands than values");
            }
            else
            {
                throw new Exception("Unknown token in expression");
            }

            return true;
        }

        
        void Reset()
        {
            operatorStack.Clear();
            operandStack.Clear();
            pos = 0;
        }

        bool Do_npr()
        {
            if (operatorStack.Count == 0)
                return false;

            Operator oper = operatorStack.Pop();

            if (operandStack.Count == 0 || oper == Operator.ParenthesisBegin)
                return false;

            Boolean a, b;

            switch (oper)
            {
                case Operator.UnaryNot:
                    a = !operandStack.Pop();
                    operandStack.Push(a);
                    break;

                case Operator.And:
                    a = operandStack.Pop();

                    if (operandStack.Count == 0)
                        return false;
                    b = operandStack.Pop();

                    operandStack.Push(a & b);
                    break;

                case Operator.Xor:
                    a = operandStack.Pop();

                    if (operandStack.Count == 0)
                        return false;
                    b = operandStack.Pop();

                    operandStack.Push(a ^ b);
                    break;

                case Operator.Or:
                    a = operandStack.Pop();

                    if (operandStack.Count == 0)
                        return false;
                    b = operandStack.Pop();

                    operandStack.Push(a | b);
                    break;
            }

            return true;
        }

        private readonly string expression;
        private readonly List<bool> criteriaValues;

        private int pos;
        private string token;
        private Type type;
        private Boolean curOperand;
        private Operator curOperator;

        private readonly Stack<Operator> operatorStack = new Stack<Operator>(30);
        private readonly Stack<bool> operandStack = new Stack<bool>(30);
        private readonly Regex regexp = new Regex("\\S");
        private readonly Regex regexp2 = new Regex("[ \\(\\)]");

    }
}