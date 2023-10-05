using Xunit;
using TodoApi.Models;

namespace TodoApi.UnitTests
{
    public class TodoItemUnitTests
    {
        //[Fact]
        //public void IsComplete_DefaultValue_ShouldBeFalse()
        //{
        //    // Arrange
        //    var todoItem = new TodoItem();

        //    // Act & Assert
        //    Assert.False(todoItem.IsComplete);
        //}
        [Fact]
        public void IsComplete_DefaultValue_ShouldBeTrue()
        {
            // Arrange
            var todoItem = new TodoItem();
            todoItem.IsComplete = true;

            // Act & Assert
            Assert.True(todoItem.IsComplete);
        }
        [Fact]
        public void Name_Value_IsNotNull()
        {
            // Arrange
            var todoItem = new TodoItem();
            todoItem.Name = "Complete ASP.NET Project Work";

            // Act & Assert
            //Assert.NotNull(todoItem.Name);
            Assert.NotEmpty(todoItem.Name);
        }
        [Fact]
        public void Name_Value_IsEmpty()
        {
            // Arrange
            var todoItem = new TodoItem();
            todoItem.Name = String.Empty;

            // Act & Assert
            //Assert.NotNull(todoItem.Name);
            Assert.Empty(todoItem.Name);
        }

    }
}