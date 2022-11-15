using AddressBookAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace AddressBookUnitTest
{
    public class UnitTest1 //: IClassFixture<AddressBookController>
    {
        private readonly AddressBookController _addresBookController;
        private readonly AddressBookClassFake _service;

        public UnitTest1()
        {
            _service = new AddressBookClassFake();
        //    _addresBookController = new AddressBookController(_service); 
        }

 

        [Fact]
        public  async  void Test1()
        {
            var response =await  _addresBookController.GetAddressBookCount() as OkObjectResult;

            var items = Assert.IsType<int>(response.Value);
            Assert.Equal(2, items);
        }
    }
}
