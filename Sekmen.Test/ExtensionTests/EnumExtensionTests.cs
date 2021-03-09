using Sekmen.Core.Extensions;
using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace Sekmen.Test.ExtensionTests
{
    public class EnumExtensionTests
    {
        [Fact]
        public void GetAllItemsTest()
        {
            List<TestEnum> result = EnumExtensions.GetAllItems<TestEnum>();
            result.Count.ShouldBeGreaterThanOrEqualTo(1);
        }

        public enum TestEnum
        {
            item1,
            item2,
            item3
        }
    }
}
