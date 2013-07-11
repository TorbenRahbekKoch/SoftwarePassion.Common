using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SoftwarePassion.Common.Core.Utilities
{
    /// <summary>
    /// Given two operands and an operator the entire expression is evaluated to true or false.
    /// I made this class years ago, simply holding on to it to see how my coding style etc. has changed over
    /// time. I have no idea whether I've ever used it ;)
    /// </summary>
    public static class CriterionEvaluator
    {
        /// <summary>
        /// Evaluates the criterion in a case-insensitive way.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="matchOperator">The match operator.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <returns></returns>
        public static bool EvaluateCriterion(string leftOperand, string matchOperator, string rightOperand)
        {
            return EvaluateCriterion(leftOperand, matchOperator, rightOperand, Case.Insensitive, "[default]");
        }

        /// <summary>
        /// Evaluates the criterion with the given case sensivity setting.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="matchOperator">The match operator.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <param name="caseSensitive">The case sensitive.</param>
        /// <returns></returns>
        public static bool EvaluateCriterion(string leftOperand, string matchOperator, string rightOperand, Case caseSensitive)
        {
            return EvaluateCriterion(leftOperand, matchOperator, rightOperand, caseSensitive, "[default]");
        }

        /// <summary>
        /// Evaluates the criterion with the given case sensivity setting and culture.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="matchOperator">The match operator.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <param name="caseSensitive">The case sensitive.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static bool EvaluateCriterion(string leftOperand, string matchOperator, string rightOperand, Case caseSensitive, string culture)
        {
            CultureInfo cultureInfo;
            if (culture == "[default]")
                cultureInfo = CultureInfo.CurrentCulture;
            else
                cultureInfo = new CultureInfo(culture);
            
            return EvaluateCriterion(leftOperand, matchOperator, rightOperand, caseSensitive, cultureInfo);
        }

        /// <summary>
        /// Evaluates the criterion.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="matchOperator">The match operator.</param>
        /// <param name="rightOperand">The right operand.</param>
        /// <param name="cultureInfo">The culture info.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Illegal operator  + matchOperator</exception>
        public static bool EvaluateCriterion(string leftOperand, string matchOperator, string rightOperand, Case caseSensitive, CultureInfo cultureInfo)
        {
            CompareOptions compareOptions = caseSensitive == Case.Sensitive 
                ? CompareOptions.None 
                : CompareOptions.IgnoreCase;            

            CompareInfo ci = cultureInfo.CompareInfo;
            if (matchOperator == "=")
                return ci.Compare(leftOperand, rightOperand, compareOptions) == 0;
            if (matchOperator == "<")
                return ci.Compare(leftOperand, rightOperand, compareOptions) < 0;
            if (matchOperator == ">")
                return ci.Compare(leftOperand, rightOperand, compareOptions) > 0;
            if (matchOperator == "<=")
                return ci.Compare(leftOperand, rightOperand, compareOptions) <= 0;
            if (matchOperator == ">=")
                return ci.Compare(leftOperand, rightOperand, compareOptions) >= 0;
            if (matchOperator == "<>")
                return ci.Compare(leftOperand, rightOperand, compareOptions) != 0;

            RegexOptions regexOptions = caseSensitive == Case.Sensitive
                ? RegexOptions.None
                : RegexOptions.IgnoreCase;

            if (matchOperator == "like")
            {
                Regex reg = new Regex(rightOperand, regexOptions);
                return (reg.Match(leftOperand).Success);
            }
            if (matchOperator == "not like")
            {
                Regex reg = new Regex(rightOperand, regexOptions);
                return (!reg.Match(leftOperand).Success);
            }

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
            if (matchOperator == "n=") // n => numerical comparison instead of string comparison
                return lValue == rValue;
            if (matchOperator == "n<")
            {
                bool t = lValue < rValue;
                return lValue < rValue;
            }
            if (matchOperator == "n>")
                return lValue > rValue;
            if (matchOperator == "n<=")
                return (lValue <= rValue);
            if (matchOperator == "n>=")
                return (lValue >= rValue);
            if (matchOperator == "n<>")
                return (lValue != rValue);
            throw new Exception("Illegal operator " + matchOperator);
        }
    }
}
