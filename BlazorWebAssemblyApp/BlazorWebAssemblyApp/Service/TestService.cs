using Blazored.LocalStorage;

namespace BlazorWebAssemblyApp.Service
{
    public class TestService
    {
        private readonly ILocalStorageService _localStorage;
        public TestService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task setItem()
        {
            await _localStorage.SetItemAsync("hello", "Xin chao");
        }
    }
}
