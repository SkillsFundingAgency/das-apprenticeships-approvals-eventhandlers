using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            true.Should().BeTrue();
        }
    }
}