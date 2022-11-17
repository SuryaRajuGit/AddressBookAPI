using AddressBookAPI.Controllers;
using AddressBookAPI.Data;
using AddressBookAPI.Models;
using AddressBookAPI.Repository;
using AddressBookAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Xunit;

namespace AddressBookUnitTest
{
    public class UnitTest1
    {
        private readonly AddressBookController _addresBookController;
        private readonly IAddressBookRepository _repository;
        private readonly AddressBookServices _addressBookServices;
        private readonly IConfiguration _configuration;
        //  private readonly AddressBookContext _context;
        public UnitTest1()
        {
            _repository = new AddressBookClassFake();
            _addressBookServices = new AddressBookServices(_configuration, _repository);
            _addresBookController = new AddressBookController(_addressBookServices);

        }
        [Fact]
        public async void Get_number_of_accounts_Test()
        {
            var response = await _addresBookController.GetAddressBookCount() as OkObjectResult;
            var items = Assert.IsType<int>(response.Value);
            Assert.Equal(2, items);
        }

        [Fact]
        public async void Get_all_accounts_Test()
        {
            var okResult = await _addresBookController.GetAllAddressBooksAccounts(4, "ad2", "firstName", "ASC");

            //Assert.IsType<OkObjectResult>(okResult as OkObjectResult);

          //  var result = _controller.Get();
            //Assert
            Assert.IsType<OkObjectResult>(okResult.Result);

            var list = okResult.Result as OkObjectResult;

            Assert.IsType<List<userDTO>>(list.Value);

            var listBooks = list.Value as List<userDTO>;

            Assert.Equal(2, listBooks.Count);
            // Assert
            //   var items = Assert.IsType<List<userDTO>>(okResult.Value);
        }

        [Theory]
        [InlineData("D1E91040-8C5E-4748-A625-2E493C7818D9", "E6E91040-8C5E-4748-A625-2E493C7818D9")]
        public async void Get_account(string id,string inv)
        {
            var gu = new Guid(id);
            var invv = new Guid(inv);
            //    var result = _addresBookController.GetAddressBook(gu);
            //Asser

            var notFoundResult =await _addresBookController.GetAddressBook(invv);
            var okResult =await _addresBookController.GetAddressBook(gu);

            Assert.IsType<NotFoundResult>(notFoundResult.Result);
            Assert.IsType<OkObjectResult>(okResult.Result);

            var item = okResult.Result as OkObjectResult;

            //We Expect to return a single book
            Assert.IsType<userDTO>(item.Value);

            //Now, let us check the value itself.
            var bookItem = item.Value as userDTO;
            Assert.Equal(gu, bookItem.Id);
            Assert.Equal("abc", bookItem.firstName);
        }
    }
}
