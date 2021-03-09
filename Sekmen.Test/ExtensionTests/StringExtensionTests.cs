using Sekmen.Core.Extensions;
using Shouldly;
using Xunit;

namespace Sekmen.Test.ExtensionTests
{
    public class StringExtensionTests
    {
        [Fact]
        public void LeftTest()
        {
            "1234567890".Left(1).ShouldBe("1");
            "1234567890".Left(5).ShouldBe("12345");
        }

        [Fact]
        public void RightTest()
        {
            "1234567890".Right(1).ShouldBe("0");
            "1234567890".Right(5).ShouldBe("67890");
        }

        [Fact]
        public void RemoveFromStartTest()
        {
            "1234567890".RemoveFromStart().ShouldBe("234567890");
            "1234567890".RemoveFromStart(5).ShouldBe("67890");
            "1234567890".RemoveFromStart(50).ShouldBe("");
        }

        [Fact]
        public void RemoveFromEndTest()
        {
            "1234567890".RemoveFromEnd().ShouldBe("123456789");
            "1234567890".RemoveFromEnd(5).ShouldBe("12345");
            "1234567890".RemoveFromEnd(50).ShouldBe("");
        }

        [Fact]
        public void TruncateTest()
        {
            "1234567890".Truncate().ShouldBe("1234567890");
            new string('0', 100).Truncate().ShouldBe(new string('0', 100));
            new string('0', 150).Truncate().ShouldBe(new string('0', 100) + "...");
            "1234567890".Truncate(5).ShouldBe("12345...");
            "1234567890".Truncate(5, "").ShouldBe("12345");
        }

        [Fact]
        public void WordCountTest()
        {
            "1 2 3 4 5 6 7 8 9 0".WordCount().ShouldBe(10);
            "1234567890".WordCount().ShouldBe(1);
            "".WordCount().ShouldBe(0);
        }

        [Fact]
        public void IsValidIpTest()
        {
            //sadece IP 4 yeterli
            "1.1.1.1".IsValidIp().ShouldBeTrue();
            "255.255.255.255".IsValidIp().ShouldBeTrue();
            "1.2.3.x".IsValidIp().ShouldBeFalse();
            "8,8,8,8".IsValidIp().ShouldBeFalse();
        }

        [Fact]
        public void IsValidPhoneTest()
        {
            "00 90 507 123 45 45".IsValidPhone().ShouldBeTrue();
            "0090 507 123 45 45".IsValidPhone().ShouldBeTrue();
            "+90 507 123 45 45".IsValidPhone().ShouldBeTrue();
            "+90 507 123 4545".IsValidPhone().ShouldBeTrue();
            "+90 507 1234545".IsValidPhone().ShouldBeTrue();
            "+90 5071234545".IsValidPhone().ShouldBeTrue();
            "+905071234545".IsValidPhone().ShouldBeTrue();
            "0 507 123 45 45".IsValidPhone().ShouldBeTrue();
            "0 507 123 4545".IsValidPhone().ShouldBeTrue();
            "0507 123 45 45".IsValidPhone().ShouldBeTrue();
            "0507 123 4545".IsValidPhone().ShouldBeTrue();
            "05071234545".IsValidPhone().ShouldBeTrue();
        }

        [Fact]
        public void IsValidUrlTest()
        {
            "https://www.crema.com.tr/".IsValidUrl().ShouldBeTrue();
            "http://test.isvarant.com/".IsValidUrl().ShouldBeTrue();
            "isvarant.com/".IsValidUrl().ShouldBeFalse();
        }

        [Fact]
        public void IsTcValidTest()
        {
            "12345678950".IsTcValid().ShouldBeTrue();
            "12345678900".IsTcValid().ShouldBeFalse();
        }

        [Fact]
        public void RemoveHtmlTagsTest()
        {
            "<p></p>".RemoveHtmlTags().ShouldBe("");
            "<p>1</p>".RemoveHtmlTags().ShouldBe("1");
            "1<3".RemoveHtmlTags().ShouldBe("1<3");
        }

        [Fact]
        public void FixColNamesTest()
        {
            "KISA KOD".FixColNames().ShouldBe("KisaKod");
            "İHRAÇ MİKTARI".FixColNames().ShouldBe("IhracMiktari");
            "TÜR".FixColNames().ShouldBe("Tur");
            "İLK MAKSİMUM LOT".FixColNames().ShouldBe("IlkMaksimumLot");
            "ÇARPAN".FixColNames().ShouldBe("Carpan");
            "İŞLEME KOYMA FİYATI/SEVİYESİ".FixColNames().ShouldBe("IslemeKoymaFiyatiSeviyesi");
            "SON İŞLEM TARİHİ".FixColNames().ShouldBe("SonIslemTarihi");
        }

        [Fact]
        public void ToTitleCaseTest()
        {
            "abcd".ToTitleCase().ShouldBe("Abcd");
            "abab cdcd".ToTitleCase().ShouldBe("Abab Cdcd");
            "AB CD EF".ToTitleCase().ShouldBe("AB CD EF");
            "Abc def GH".ToTitleCase().ShouldBe("Abc Def GH");
        }

        [Fact]
        public void ToCamelCaseTest()
        {
            "abCd".ToCamelCase().ShouldBe("Abcd");
            "abcd".ToCamelCase().ShouldBe("Abcd");
            "abAb cDcd".ToCamelCase().ShouldBe("Abab Cdcd");
            "AB CD EF".ToCamelCase().ShouldBe("Ab Cd Ef");
            "Abc def GH".ToCamelCase().ShouldBe("Abc Def Gh");
        }

        [Fact]
        public void FixTurkishLettersTest()
        {
            "qwertyuıopğü".FixTurkishLetters().ShouldBe("qwertyuiopgu");
            "asdfghjklşi".FixTurkishLetters().ShouldBe("asdfghjklsi");
            "zxcvbnmöç".FixTurkishLetters().ShouldBe("zxcvbnmoc");
            "QWERTYUIOPĞÜ".FixTurkishLetters().ShouldBe("QWERTYUIOPGU");
            "ASDFGHJKLŞİ".FixTurkishLetters().ShouldBe("ASDFGHJKLSI");
            "ZXCVBNMÖÇ".FixTurkishLetters().ShouldBe("ZXCVBNMOC");
        }
    }
}
