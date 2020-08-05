using challenge.Controllers;
using challenge.Data;
using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var compensation = new Compensation()
            {
                Salary = 120000,
                EffectiveDate = new DateTime(2020, 8, 5)
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync($"api/employee/{employeeId}/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(newCompensation.EmployeeId, employeeId);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
        }

        [TestMethod]
        public void CreateCompensation_Returns_BadRequest()
        {
            // Arrange
            var employeeId = "b7839309-3348-463b-a7e3-5de1c168beb3";
            var compensation = new Compensation() // Seed value
            {
                Salary = 85000,
                EffectiveDate = new DateTime(2020, 8, 2)
            };

            var newCompensation = new Compensation()
            {
                Salary = 76000,
                EffectiveDate = new DateTime(2020, 8, 5)
            };

            var firstRequestContent = new JsonSerialization().ToJson(compensation);
            var secondRequestContent = new JsonSerialization().ToJson(newCompensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync($"api/employee/{employeeId}/compensation",
               new StringContent(firstRequestContent, Encoding.UTF8, "application/json")); // Adds seed value
            var firstResponse = postRequestTask.Result;

            postRequestTask = _httpClient.PostAsync($"api/employee/{employeeId}/compensation",
               new StringContent(secondRequestContent, Encoding.UTF8, "application/json")); // actual test
            var secondResponse = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, firstResponse.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, secondResponse.StatusCode);
        }

        [TestMethod]
        public void GetByEmployeeId_Returns_Ok()
        {
            // Arrange
            var employeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f";
            var compensation = new Compensation() // Seed value
            {
                Salary = 85000,
                EffectiveDate = new DateTime(2020, 8, 2)
            };

            var firstRequestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync($"api/employee/{employeeId}/compensation",
               new StringContent(firstRequestContent, Encoding.UTF8, "application/json")); // Adds seed value
            postRequestTask.Wait();

            var getRequestTask = _httpClient.GetAsync($"api/employee/{employeeId}/compensation"); // actual test
            var response = getRequestTask.Result;

            var retComp = response.DeserializeContent<Compensation>();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(employeeId, retComp.EmployeeId);
            Assert.AreEqual(compensation.Salary, retComp.Salary);
            Assert.AreEqual(compensation.EffectiveDate, retComp.EffectiveDate);
        }
    }
}
