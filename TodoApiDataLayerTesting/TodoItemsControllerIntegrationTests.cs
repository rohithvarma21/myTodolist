using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Models;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using System.Net.Http.Json;

namespace TodoApiDataLayerTesting
{
    public class TodoItemsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public TodoItemsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        [Fact]
        public async Task GetTodoItems_ReturnsListOfTodoItems()
        {
            //Arrange
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
            var client = _factory.CreateClient();
            dbContext.TodoItems.RemoveRange(dbContext.TodoItems);
            await dbContext.SaveChangesAsync();
            dbContext.TodoItems.Add(new TodoItem { Name = "Test Todo 1", IsComplete = false });
            dbContext.TodoItems.Add(new TodoItem { Name = "Test Todo 2", IsComplete = true });
            await dbContext.SaveChangesAsync();
            //Act
            var response = await client.GetAsync("/api/todoitems");
            var responseContent = await response.Content.ReadAsStringAsync();
            var todoItems = JsonConvert.DeserializeObject<List<TodoItem>>(responseContent);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            //todoItems.Should().NotBeNullOrEmpty();
            todoItems.Count.Should().Be(2);
            todoItems.Should().Contain(ti => ti.Name =="Test Todo 1" && ti.IsComplete == false);
            todoItems.Should().Contain(ti => ti.Name =="Test Todo 2" && ti.IsComplete == true);
        }
        [Fact]
        public async Task GetTodoItemWithId_ReturnsSuccessStatusCode()
        {
            //Arrange
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
            var client = _factory.CreateClient();
            var todoItem = new TodoItem { Name="Test Todo", IsComplete=false };
            dbContext.TodoItems.Add(todoItem);
            await dbContext.SaveChangesAsync();
            //Act 
            var response = await client.GetAsync($"/api/todoitems/{todoItem.Id}");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetTodoItemWithId_ReturnsNotFoundStatusCode()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync("/api/todoitems/123");
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostTodoItemAsync_ShouldCreateNewTodoItem()
        {
            //Arrange
            var client = _factory.CreateClient();
            var todoItem = new TodoItem
            {
                Name = "New Todo Item",
                IsComplete = false
            };
            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(todoItem), Encoding.UTF8, "application/json");
            //Act
            var response = await client.PostAsync("/api/todoitems", jsonContent);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseContent = await response.Content.ReadAsStringAsync(); 
            var createdTodoItem = System.Text.Json.JsonSerializer.Deserialize<TodoItem>(responseContent,new JsonSerializerOptions { PropertyNameCaseInsensitive = true});   
            createdTodoItem.Should().NotBeNull();
            createdTodoItem.Name.Should().Be(todoItem.Name);
            createdTodoItem.IsComplete.Should().Be(todoItem.IsComplete);
        }
        [Fact]
        public async Task PutTodoItem_Should_Update_Existing_TodoItem()
        {
            //Arrange
            var client = _factory.CreateClient();
            var newTodoItem = new TodoItem { Name = "Test Todo Item" };
            var response = await client.PostAsJsonAsync("/api/TodoItems", newTodoItem);
            var createdTodoItem = await response.Content.ReadFromJsonAsync<TodoItem>();
            //Modify the todo item
            createdTodoItem.Name = "Updated Todo Item";
            //Act 
            var putResponse = await client.PutAsJsonAsync($"/api/TodoItems/{createdTodoItem.Id}",createdTodoItem);
            //Assert
            putResponse.EnsureSuccessStatusCode();
            var getResponse = await client.GetAsync($"/api/TodoItems/{createdTodoItem.Id}");
            var updatedTodoItem = await getResponse.Content.ReadFromJsonAsync<TodoItem>();
            createdTodoItem.Id.Should().Be(updatedTodoItem.Id);
            createdTodoItem.Name.Should().Be(updatedTodoItem.Name);
        }
        [Fact]
        public async Task DeleteTodoItem_DeletesExistingTodoItem()
        {
            //Arrange
            var client = _factory.CreateClient();
            var newTodoItem = new TodoItem { Name="Test Todo Item" };
            var response = await client.PostAsJsonAsync("api/TodoItems", newTodoItem);
            var createdTodoItem = await response.Content.ReadFromJsonAsync<TodoItem>();
            //Act 
            var createdId = createdTodoItem.Id;
            response = await client.DeleteAsync($"/api/TodoItems/{createdId}");
            response.EnsureSuccessStatusCode() ;
            //Assert
            response = await client.GetAsync($"/api/TodoItems/{createdId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);  


        }
    }
}
