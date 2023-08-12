using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Models;
using System.Net;

namespace TodoApiDataLayerTesting
{
    public class EdgeCasesTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public EdgeCasesTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        [Fact]
        public async Task PostTodoItemAsync_InvalidInput_ReturnsBadRequest()
        {
            //Arrange
            var client = _factory.CreateClient();
            var invalidTodoItem = new TodoItem()
            {
                Name = null,
                IsComplete = false
            };
            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(invalidTodoItem), Encoding.UTF8, "application/json");
            //Act
            var response = await client.PostAsync("/api/todoitems", jsonContent);
            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task GetTodoItemWithInvalidId_ReturnsNotFound()
        {
            //Arrange
            var client = _factory.CreateClient();
            int invalidId = 1000;
            //Act
            var response = await client.GetAsync($"/api/todoitems/{invalidId}");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }
        [Fact]
        public async Task PutTodoItem_WithInvalidId_ReturnsNotFound()
        {
            //Arrange
            var client = _factory.CreateClient();
            var invalidTodoItem = new TodoItem
            {
                Id= 1500,
                Name = "Updated Todo Item",
                IsComplete = true

            };
            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(invalidTodoItem), Encoding.UTF8, "application/json");
            //Act
            var response = await client.PutAsync($"/api/todoitems/{invalidTodoItem.Id}",jsonContent);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
