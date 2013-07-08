using System;
using NUnit.Framework;
using SoftwarePassion.Common.Core.Utilities;

namespace SoftwarePassion.Common.Core.Tests.Unit.Utilities
{
    [TestFixture]
    public class CriterionEvaluatorTest
    {
        [Test]
        public void EvaluateCriterionTest()
        {
            string leftOperand = "value";
            string rightOperand = "value";
            Case caseSensitive = Case.Insensitive;
            string culture = "da-DK";

            Assert.IsTrue(CriterionEvaluator.EvaluateCriterion(leftOperand, "=", rightOperand));
            Assert.IsTrue(CriterionEvaluator.EvaluateCriterion(leftOperand, "=", rightOperand, caseSensitive));

            Assert.IsTrue(CriterionEvaluator.EvaluateCriterion(leftOperand, "=", rightOperand, caseSensitive, culture));
            Assert.IsFalse(CriterionEvaluator.EvaluateCriterion(leftOperand, "<", rightOperand, caseSensitive, culture));
            Assert.IsFalse(CriterionEvaluator.EvaluateCriterion(leftOperand, ">", rightOperand, caseSensitive, culture));
            Assert.IsTrue(CriterionEvaluator.EvaluateCriterion(leftOperand, "<=", rightOperand, caseSensitive, culture));
            Assert.IsTrue(CriterionEvaluator.EvaluateCriterion(leftOperand, ">=", rightOperand, caseSensitive, culture));
            Assert.IsFalse(CriterionEvaluator.EvaluateCriterion(leftOperand, "<>", rightOperand, caseSensitive, culture));

            Assert.IsTrue(CriterionEvaluator.EvaluateCriterion("SomeThing", "like", ".ome.*", caseSensitive, culture));
            Assert.IsFalse(CriterionEvaluator.EvaluateCriterion("SomeThing", "not like", ".ome.*", caseSensitive, culture));

            Assert.IsFalse(CriterionEvaluator.EvaluateCriterion(leftOperand, "n<>", rightOperand, caseSensitive, culture));

            leftOperand = "27,14";
            rightOperand = "27,15";
            Assert.IsFalse(CriterionEvaluator.EvaluateCriterion(leftOperand, "n=", rightOperand, caseSensitive, culture));
            Assert.IsTrue(CriterionEvaluator.EvaluateCriterion(leftOperand, "n<", rightOperand, caseSensitive, culture));
            Assert.IsFalse(CriterionEvaluator.EvaluateCriterion(leftOperand, "n>", rightOperand, caseSensitive, culture));
            Assert.IsTrue(CriterionEvaluator.EvaluateCriterion(leftOperand, "n<=", rightOperand, caseSensitive, culture));
            Assert.IsFalse(CriterionEvaluator.EvaluateCriterion(leftOperand, "n>=", rightOperand, caseSensitive, culture));
            Assert.IsTrue(CriterionEvaluator.EvaluateCriterion(leftOperand, "n<>", rightOperand, caseSensitive, culture));

            // Illegal operator test
            bool hasThrown = false;
            try
            {
                Assert.IsFalse(CriterionEvaluator.EvaluateCriterion(leftOperand, "¤¤", rightOperand, caseSensitive, culture));
            }
            catch (Exception)
            {
                hasThrown = true;
            }
            Assert.IsTrue(hasThrown);

            leftOperand = "27,16";
            rightOperand = "27,15";
            Assert.IsFalse(CriterionEvaluator.EvaluateCriterion(leftOperand, "n=", rightOperand, caseSensitive, culture));
            Assert.IsFalse(CriterionEvaluator.EvaluateCriterion(leftOperand, "n<", rightOperand, caseSensitive, culture));
            Assert.IsTrue(CriterionEvaluator.EvaluateCriterion(leftOperand, "n>", rightOperand, caseSensitive, culture));
            Assert.IsFalse(CriterionEvaluator.EvaluateCriterion(leftOperand, "n<=", rightOperand, caseSensitive, culture));
            Assert.IsTrue(CriterionEvaluator.EvaluateCriterion(leftOperand, "n>=", rightOperand, caseSensitive, culture));
            Assert.IsTrue(CriterionEvaluator.EvaluateCriterion(leftOperand, "n<>", rightOperand, caseSensitive, culture));
        }   

    }
}
