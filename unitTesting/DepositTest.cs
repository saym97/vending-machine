using coreServices.Services.User;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unitTesting
{
    public class DepositTest : DISetup
    {
        public IUserService _userService;

        public DepositTest(): base()
        {
            _userService = (IUserService) _serviceProvider.GetService(typeof(IUserService));
        }

        [Test]
        public void Test_ConfigService()
        {
            Assert.IsNotNull(_userService);
        }


        Guid id = new Guid("2F4C5D0A-7513-4347-9917-2ABBF3A231DF");

        [Test]
        public void Test_EmptyDeposit()
        {
            var res = _userService.Deposit(id, 0);
            Assert.IsTrue(res.Message == "You can only deposit the coins of 5, 10, 20, 50 and 100 cents" );
        }

        [Test]
        public void Test_DepositCoinOtherThanRequired()
        {
            var res = _userService.Deposit(id, 30 );
            Assert.IsTrue(res.Message == "You can only deposit the coins of 5, 10, 20, 50 and 100 cents");

        }

        [Test]
        public void Test_DepositCorrectCoin()
        {
            var res = _userService.Deposit(id, 50);
            Assert.IsTrue(res.Success);
        }

        // update-username

        [Test]
        public void Test_UsernameAlreadyExist()
        {
            var res = _userService.UpdateUsername(id, "usama_seller@mail.com");

            Assert.IsFalse(res.Success);
        }
    }
}
