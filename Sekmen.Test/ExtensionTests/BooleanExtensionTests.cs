using Sekmen.Core.Extensions;
using Shouldly;
using Xunit;

namespace Sekmen.Test.ExtensionTests
{
    public class BooleanExtensionTests
    {
        [Fact]
        public void ToBoolTest()
        {
            //false
            "False".ToBool().ShouldBeFalse();
            false.ToBool().ShouldBeFalse();
            "false".ToBool().ShouldBeFalse();
            "0".ToBool().ShouldBeFalse();
            0.ToBool().ShouldBeFalse();
            //true
            true.ToBool().ShouldBeTrue();
            "True".ToBool().ShouldBeTrue();
            "true".ToBool().ShouldBeTrue();
            "1".ToBool().ShouldBeTrue();
            1.ToBool().ShouldBeTrue();
            //others
            "te".ToBool().ShouldBeFalse();
            "".ToBool().ShouldBeFalse();
        }

        [Fact]
        public void IsNullOrEmptyTest()
        {
            string i = null;
            //null
            // ReSharper disable once ExpressionIsAlwaysNull
            i.IsNullOrEmpty().ShouldBeTrue();
            "".IsNullOrEmpty().ShouldBeTrue();
            " ".IsNullOrEmpty().ShouldBeTrue();
            //not null
            "q".IsNullOrEmpty().ShouldBeFalse();
            "%".IsNullOrEmpty().ShouldBeFalse();
            "?".IsNullOrEmpty().ShouldBeFalse();
            "*".IsNullOrEmpty().ShouldBeFalse();
            "\\".IsNullOrEmpty().ShouldBeFalse();
            "/".IsNullOrEmpty().ShouldBeFalse();
        }

        [Fact]
        public void IsEmailTest()
        {
            //email
            "cdid-0000262752+alhajj=live.com@crema.com.tr".IsEmail().ShouldBeTrue();
            "cdid-0000132312+lars.olaison=infect.gu.se@mailspor.istanbul".IsEmail().ShouldBeTrue();
            "yazilim@crema.com.tr".IsEmail().ShouldBeTrue();
            "alhajj@live.com".IsEmail().ShouldBeTrue();
            "0000262752+alhajj@live.com".IsEmail().ShouldBeTrue();
            //not an email
            //"0000262752+alhajj@asdasffrdasd.com".IsEmail().ShouldBeFalse();
            "alhajj=live.com".IsEmail().ShouldBeFalse();
            "alhajj@live".IsEmail().ShouldBeFalse();
            "alhajj@".IsEmail().ShouldBeFalse();
            "live.com".IsEmail().ShouldBeFalse();
            "alhajj=live".IsEmail().ShouldBeFalse();
        }

        [Fact]
        public void IsNumericTest()
        {
            bool test = "123452345".IsNumeric();
            test.ShouldBeTrue();
            123452345.IsNumeric();
            test.ShouldBeTrue();
            test = "12345s2345".IsNumeric();
            test.ShouldBeFalse();
            test = "asdf".IsNumeric();
            test.ShouldBeFalse();
        }
    }
}
