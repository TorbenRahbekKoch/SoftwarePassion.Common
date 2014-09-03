using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;

namespace SoftwarePassion.Common.Core.ExpressionEvaluating
{
    /// <summary>
    /// Enables to evaluate an expression like 'A OR B AND (D XOR E)'
    /// where each letter indicates the boolean result of some other evaluation.
    /// </summary>
    public static class ExpressionEvaluator
    {
        /// <summary>
        /// Evaluates the specified expression value.
        /// </summary>
        /// <param name="expression">The logical expression.</param>
        /// <param name="criteriaValues">The list of values, first index is A, second is B and so on.</param>
        /// <returns>A boolean value representing the result</returns>
        public static bool Evaluate(string expression, IEnumerable<bool> criteriaValues)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(expression));
            Contract.Requires(criteriaValues != null);

            var evaluator = new ExpressionEvaluatorImplementation(expression, criteriaValues);
            return evaluator.Eval();
        }
    }
}
