using Sekmen.Core.Extensions;
using Shouldly;
using System;
using Xunit;

namespace Sekmen.Test.ExtensionTests
{
    public class DateTimeExtensionTests
    {
        [Fact]
        public void ToDateTimeTest()
        {
            "2019".ToDateTime().ShouldBe(new DateTime());
            "".ToDateTime().ShouldBe(new DateTime());
            "01/02/2019".ToDateTime().ShouldBe(new DateTime(2019, 2, 1));
            "01-02-2019".ToDateTime().ShouldBe(new DateTime(2019, 2, 1));
            "01.10.2019".ToDateTime().ShouldBe(new DateTime(2019, 10, 1));
            "01/10/2019 12:25:35".ToDateTime().ShouldBe(new DateTime(2019, 10, 1, 12, 25, 35));
        }

        [Fact]
        public void IsDateTest()
        {
            "2019".IsDate().ShouldBeFalse();
            "".IsDate().ShouldBeFalse();
            "01/02/2019".IsDate("dd/MM/yyyy").ShouldBeTrue();
            "01-02-2019".IsDate("dd-MM-yyyy").ShouldBeTrue();
            "2019-02-01".IsDate("yyyy-MM-dd").ShouldBeTrue();
            "01.10.2019".IsDate().ShouldBeTrue();
            "01.10.2019 12:25:35".IsDate("dd.MM.yyyy HH:mm:ss").ShouldBeTrue();
            "01/10/2019 12:25:35".IsDate("dd/MM/yyyy HH:mm:ss").ShouldBeTrue();
        }

        [Fact]
        public void FormatUniversalTest()
        {
            new DateTime(2012, 2, 1).FormatUniversal().ShouldBe("2012-02-01");
            new DateTime(2012, 2, 1, 14, 15, 16).FormatUniversal().ShouldBe("2012-02-01");
            new DateTime(2012, 2, 3, 4, 15, 16).FormatUniversal().ShouldBe("2012-02-03");
        }

        [Fact]
        public void FormatUniversalWithTimeTest()
        {
            new DateTime(2012, 2, 1).FormatUniversalWithTime().ShouldBe("2012-02-01 00:00:00");
            new DateTime(2012, 2, 1, 14, 15, 16).FormatUniversalWithTime().ShouldBe("2012-02-01 14:15:16");
            new DateTime(2012, 2, 3, 4, 5, 6).FormatUniversalWithTime().ShouldBe("2012-02-03 04:05:06");
        }

        [Fact]
        public void FormatWithTimeTest()
        {
            new DateTime(2012, 2, 1).FormatWithTime().ShouldBe("01.02.2012 00:00:00");
            new DateTime(2012, 2, 1, 14, 15, 16).FormatWithTime().ShouldBe("01.02.2012 14:15:16");
            new DateTime(2012, 2, 1, 14, 15, 16).FormatWithTime("yyyy-MM-dd").ShouldBe("2012-02-01");
            new DateTime(2012, 2, 3, 4, 15, 16).FormatWithTime("dd.MM.yyyy").ShouldBe("03.02.2012");
            new DateTime(2012, 2, 3).FormatWithTime("dd.MM.yyyy").ShouldBe("03.02.2012");
        }

        [Fact]
        public void FormatTest()
        {
            new DateTime(2012, 2, 1).Format("dd.MM.yyyy HH:mm:ss").ShouldBe("01.02.2012 00:00:00");
            new DateTime(2012, 2, 1, 14, 15, 16).Format("dd.MM.yyyy HH:mm:ss").ShouldBe("01.02.2012 14:15:16");
            new DateTime(2012, 2, 1, 14, 15, 16).Format("yyyy-MM-dd").ShouldBe("2012-02-01");
            new DateTime(2012, 2, 3, 4, 15, 16).Format().ShouldBe("03.02.2012");
            new DateTime(2012, 2, 3).Format().ShouldBe("03.02.2012");
        }
    }
}
