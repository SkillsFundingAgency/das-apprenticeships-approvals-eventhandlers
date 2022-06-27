using System;
using System.Collections.Generic;
using AutoFixture;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests
{
    public class TestData
    {
        private readonly Dictionary<string, object> _testData;
        private readonly Fixture _fixture;
        public TestData()
        {
            _testData = new Dictionary<string, object>();
            _fixture = new Fixture();
        }

        public T GetOrCreate<T>(string? key = null, Func<T>? onCreate = null)
        {
            if (key == null)
            {
                key = typeof(T).FullName;
            }

            if (!_testData.ContainsKey(key))
            {
                if (onCreate == null)
                {
                    _testData.Add(key, _fixture.Create<T>());
                }
                else
                {
                    _testData.Add(key, onCreate.Invoke());
                }
            }

            return (T)_testData[key];
        }
    }
}
