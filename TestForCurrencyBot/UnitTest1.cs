using System.Threading.Tasks;
using CurrencyBot;
using NUnit.Framework;

namespace TestForCurrencyBot
{
    [TestFixture]
    public class TestCurrencyConverter
    {
        [Test]
        public async Task TestConvertCurrency()
        {
            CurrencyConverter testedObj=new CurrencyConverter();
            var expectedResult = 89.80700M;
            var actualResult = await testedObj.GetConvertedResult(100, Currency.Dollar, Currency.Euro);

            Assert.That(actualResult,Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task TestConvertCurrencyNegative()
        {
            CurrencyConverter testedObj = new CurrencyConverter();
            var expectedResult = 0;
            var actualResult = await testedObj.GetConvertedResult(-100, Currency.Dollar, Currency.Euro);

            Assert.That(expectedResult, Is.EqualTo(actualResult));
        }
    }
}
