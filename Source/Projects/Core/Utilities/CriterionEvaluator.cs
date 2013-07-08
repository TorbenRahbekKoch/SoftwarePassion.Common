using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SoftwarePassion.Common.Core.Utilities
{
    /// <summary>
    /// Given two operands and an operator the entire expression is evaluated to true or false.
    /// </summary>
    public static class CriterionEvaluator
    {
        public static bool EvaluateCriterion(string leftOperand, string matchOperator, string rightOperand)
        {
            return EvaluateCriterion(leftOperand, matchOperator, rightOperand, Case.Insensitive, "[default]");
        }

        public static bool EvaluateCriterion(string leftOperand, string matchOperator, string rightOperand, Case caseSensitive)
        {
            return EvaluateCriterion(leftOperand, matchOperator, rightOperand, caseSensitive, "[default]");
        }

        public static bool EvaluateCriterion(string leftOperand, string matchOperator, string rightOperand, Case caseSensitive, string culture)
        {
            CultureInfo ci;
            if (culture == "[default]")
                ci = CultureInfo.CurrentCulture;
            else
                ci = new CultureInfo(culture);
            return EvaluateCriterion(leftOperand, matchOperator, rightOperand, ci);
        }

        public static bool EvaluateCriterion(string leftOperand, string matchOperator, string rightOperand, CultureInfo cultureInfo)
        {
            CompareInfo ci = cultureInfo.CompareInfo;
            if (matchOperator == "=")
                return ci.Compare(leftOperand, rightOperand) == 0;
            else if (matchOperator == "<")  
                return ci.Compare(leftOperand, rightOperand) < 0;
            else if (matchOperator == ">")
                return ci.Compare(leftOperand, rightOperand) > 0;
            else if (matchOperator == "<=")
                return ci.Compare(leftOperand, rightOperand) <= 0;
            else if (matchOperator == ">=")
                return ci.Compare(leftOperand, rightOperand) >= 0;
            else if (matchOperator == "<>")
                return ci.Compare(leftOperand, rightOperand) != 0;
            else if (matchOperator == "like")
            {
                Regex reg = new Regex(rightOperand);
                return (reg.Match(leftOperand).Success);
            }
            else if (matchOperator == "not like")
            {
                Regex reg = new Regex(rightOperand);
                return (!reg.Match(leftOperand).Success);
            }
            else
            {
                NumberFormatInfo nfi = cultureInfo.NumberFormat;
                double lValue;
                double rValue;
                try
                {
                    lValue = Double.Parse(leftOperand, NumberStyles.Float, nfi);
                    rValue = Double.Parse(rightOperand, NumberStyles.Float, nfi);
                }
                catch
                {
                    return false;
                }
                if (matchOperator == "n=")
                    return lValue == rValue;
                else if (matchOperator == "n<")
                {
                    bool t = lValue < rValue;
                    return lValue < rValue;
                }
                else if (matchOperator == "n>")
                    return lValue > rValue;
                else if (matchOperator == "n<=")
                    return (lValue <= rValue);
                else if (matchOperator == "n>=")
                    return (lValue >= rValue);
                else if (matchOperator == "n<>")
                    return (lValue != rValue);
                else
                    throw new Exception("Illegal operator " + matchOperator);
            }
        }
    }
}
