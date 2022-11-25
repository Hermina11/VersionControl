using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestExample.Controllers;

namespace UnitTestExample
{
    class AccountControllerTestFixture
    {
        [Test,
         TestCase("abcd1234", false),
         TestCase("irf@uni-corvinus", false),
         TestCase("irf.uni-corvinus.hu", false),
         TestCase("irf@uni-corvinus.hu", true)]

        public void TestValidateEmail (string email, bool expectedResult)
        {
            var accountController = new AccountController();
            var actualResult = accountController.ValidateEmail(email);
            Assert.AreEqual(actualResult, expectedResult);
        }

        [Test, 
         TestCase("abcdABCD", false),
         TestCase("ABCD1234", false),
         TestCase("abcd1234", false),
         TestCase("Ab123", false), 
         TestCase("Abcd1234", true)]
        public void TestValidatePassword(string password, bool expectedResult) {
            var accountController = new AccountController();
            var actualResult = accountController.ValidatePassword(password);
            Assert.AreEqual(actualResult, expectedResult);

        }
    }
}
