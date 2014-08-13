using System.Runtime.ConstrainedExecution;
using System.Security;
using NUnit.Framework;
using SoftwarePassion.Common.Core.Extensions;

namespace Common.Tests.Unit.Extensions.StringExtensionTests
{
    [TestFixture]
    public class When_hashing_string
    {
        [Test]
        public void Verify_That_Safe_And_Unsafe_Implementations_Agree()
        {
            var s0 = string.Empty;
            var s1 = "1";
            var s2 = "21";
            var s3 = "321";
            var s4 = "4321";
            var s5 = "54321";
            var s6 = "654321";

            Assert.AreEqual(GetHashCode(s0), s0.SafeHash(), "Disagreement on s0");
            Assert.AreEqual(GetHashCode(s1), s1.SafeHash(), "Disagreement on s1");
            Assert.AreEqual(GetHashCode(s2), s2.SafeHash(), "Disagreement on s2");
            Assert.AreEqual(GetHashCode(s3), s3.SafeHash(), "Disagreement on s3");
            Assert.AreEqual(GetHashCode(s4), s4.SafeHash(), "Disagreement on s4");
            Assert.AreEqual(GetHashCode(s5), s5.SafeHash(), "Disagreement on s5");
            Assert.AreEqual(GetHashCode(s6), s6.SafeHash(), "Disagreement on s6");
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [SecuritySafeCritical]
        private unsafe int GetHashCode(string s)
        {
            fixed (char* chPtr = s)
            {
                int num1 = 352654597;
                int num2 = num1;
                int* numPtr = (int*)chPtr;
                int length = s.Length;
                while (length > 2)
                {
                    num1 = (num1 << 5) + num1 + (num1 >> 27) ^ *numPtr;
                    num2 = (num2 << 5) + num2 + (num2 >> 27) ^ numPtr[1];
                    numPtr += 2;
                    length -= 4;
                }
                if (length > 0)
                    num1 = (num1 << 5) + num1 + (num1 >> 27) ^ *numPtr;
                return num1 + num2 * 1566083941;
            }
        }
    }
}