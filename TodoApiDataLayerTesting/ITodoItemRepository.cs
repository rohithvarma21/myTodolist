using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApiDataLayerTesting
{
    public interface ITodoItemRepository
    {
        Task<List<TodoItem>> GetAllTodoItemsAsync();
        Task<TodoItem> GetTodoItemByIdAsync(int id);
        Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem);
        Task<TodoItem> UpdateTodoItemAsync(TodoItem todoItem);
        Task<bool> DeleteTodoItemAsync(int id);
        void Reset();
    }
}
