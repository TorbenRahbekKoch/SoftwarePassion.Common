using System;
using NUnit.Framework;
using SoftwarePassion.Common.Core.Utilities;

namespace SoftwarePassion.Common.Core.Tests.Unit.Utilities
{
    [TestFixture]
    public class ExpressionEvaluatorTest
    {
        [Test]
        public void BasicExpressions()
        {
            var evaluator = new ExpressionEvaluator();

            string expression = "A";
            bool[] values = {true, false};
            Assert.IsTrue(evaluator.Evaluate(expression, values));

            expression = "NOT A";
            Assert.IsFalse(evaluator.Evaluate(expression, values));

            // More operands than values
            values = new bool[]{ true, true };
            expression = "A AND B AND C";

            Assert.Throws<Exception>(() => evaluator.Evaluate(expression, values));

            expression = "A ¤¤ B ¤¤ C";
            bool hasThrown = false;
            try
            {
                Assert.IsTrue(evaluator.Evaluate(expression, values));
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.IsTrue(hasThrown);

            // Reset values
            values = new bool[]{ true, false };

            // Illegal AND
            expression = "A AND";
            Assert.IsFalse(evaluator.Evaluate(expression, values));

            // Illegal OR
            expression = "A OR";
            Assert.IsFalse(evaluator.Evaluate(expression, values));

            // Illegal XOR
            expression = "A XOR";
            Assert.IsFalse(evaluator.Evaluate(expression, values));

            expression = "A AND B";
            Assert.IsFalse(evaluator.Evaluate(expression, values));

            expression = "A OR B";
            Assert.IsTrue(evaluator.Evaluate(expression, values));

            expression = "A XOR B";
            Assert.IsTrue(evaluator.Evaluate(expression, values));

            expression = "NOT A OR B";
            Assert.IsFalse(evaluator.Evaluate(expression, values));

            expression = "NOT A AND B";
            Assert.IsFalse(evaluator.Evaluate(expression, values));

            expression = "NOT A XOR B";
            Assert.IsFalse(evaluator.Evaluate(expression, values));

            expression = "A OR NOT B";
            Assert.IsTrue(evaluator.Evaluate(expression, values));

            expression = "A AND NOT B";
            Assert.IsTrue(evaluator.Evaluate(expression, values));

            expression = "A XOR NOT B";
            Assert.IsFalse(evaluator.Evaluate(expression, values));
        }

        [Test]
        public void AdvancedExpressions()
        {
            ExpressionEvaluator t = new ExpressionEvaluator();

            bool[] values = new bool[] { true, false, true, true, false, true, false, true, true, false, true, false, true, true, false, true, false, true, true, false, true, false, true, true, false, true, false };

            string expression = "";
            Assert.IsFalse(t.Evaluate(expression, values));

            values = new bool[] { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            Assert.IsTrue(t.Evaluate(expression, values));

            values = new bool[] { true, false, true, true, false, true, false, true, true, false, true, false, true, true, false, true, false, true, true, false, true, false, true, true, false, true, false };

            expression = "A AND B OR C AND NOT D XOR E";
            Assert.IsFalse(t.Evaluate(expression, values));

            expression = "A AND (B OR C) AND NOT D XOR E";
            Assert.IsFalse(t.Evaluate(expression, values));

            expression = "A AND (B OR C) AND NOT (D XOR E)";
            Assert.IsFalse(t.Evaluate(expression, values));

            expression = "A AND (B OR C) OR NOT (D XOR E)";
            Assert.IsTrue(t.Evaluate(expression, values));

            expression = "A AND (B YY C) OR NOT (D XOR E)";
            Assert.IsFalse(t.Evaluate(expression, values));

            expression = "A AND NOT B XOR (NOT C XOR (D XOR E))";
            Assert.IsFalse(t.Evaluate(expression, values));

            expression = "A XOR (NOT (B XOR (NOT (C XOR (D XOR E)))))";
            Assert.IsTrue(t.Evaluate(expression, values));
        }

    }
}
