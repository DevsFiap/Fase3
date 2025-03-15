using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace TechChallengeFase02.IntegrationTests
{
    public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public ApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:8080"); // Nome do serviço do Docker
        }

        [Fact]
        public async Task Test_GetContacts_ReturnsSuccess()
        {
            var response = await _client.GetAsync("/api/contatos/buscar-todos");  
            response.EnsureSuccessStatusCode();  // Valida se o status é 200 OK

            var content = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(content); 

            Console.WriteLine("Test_GetContacts_ReturnsSuccess passou com sucesso!");
        }

        [Fact]
        public async Task Test_GetContactsByDDD_ReturnsSuccess()
        {
            var response = await _client.GetAsync("/api/contatos/buscar-ddd/11");  
            response.EnsureSuccessStatusCode();  // Valida se o status é 200 OK

            var content = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(content);

            Console.WriteLine("Test_GetContactsByDDD_ReturnsSuccess passou com sucesso!");
        }

        [Fact]
        public async Task Test_CreateContact_ReturnsSuccess()
        {
            var jsonData = JsonConvert.SerializeObject(new 
            { 
                Nome = "Nome Usuario",
                Telefone = "11987654327",
                Email = "user@example.com"
            });
            HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/contatos/criar-contato", httpContent); 
            response.EnsureSuccessStatusCode();  // Valida se o status é sucesso

            var content = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            Console.WriteLine("Test_CreateContact_ReturnsSuccess passou com sucesso!");
        }

        [Fact]
        public async Task Test_CreateContact_ReturnsErrorStatus()
        {
            var jsonData = JsonConvert.SerializeObject(new
            {
                Nome = "Nome Usuario",
                Telefone = "783899",
                Email = "user@example.com"
            });
            HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/contatos/criar-contato", httpContent);
            
            Assert.False(response.IsSuccessStatusCode);

            Console.WriteLine("Test_CreateContact_ReturnsErrorStatus passou com sucesso!");
        }
        /*
        [Fact]
        public async Task Test_UpdateContact_ReturnsSuccess()
        {
            // Arrange
            var id = 1;
            var dto = new AtualizarContatoDto
            {
                Nome = "Novo Nome",
                Telefone = "11987654321",
                Email = "email@email.com"
            };

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var contatoExistente = new Contato
            {
                Id = id,
                Nome = "Nome Antigo",
                Telefone = "11987654321",
                DataCriacao = DateTime.Now.AddDays(-1)
            };

            dbContext.Contato.Add(contatoExistente);
            await dbContext.SaveChangesAsync();

            var jsonContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/atualizar-contato/{id}", jsonContent);

            // Assert
            response.EnsureSuccessStatusCode();

            Console.WriteLine("Test_UpdateContact_ReturnsSuccess passou com sucesso!");
        }
        */


        [Fact]
        public async Task Test_UpdateContact_ReturnsError()
        {
            var jsonData = JsonConvert.SerializeObject(new
            {
                Nome = "Nome Usuario",
                Telefone = "78389945579",
                Email = "user@example.com"
            });
            HttpContent httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/contatos/atualizar-contato/0", httpContent);

            Assert.False(response.IsSuccessStatusCode);

            Console.WriteLine("Test_UpdateContact_ReturnsNotFound passou com sucesso!");
        }

      /*  [Fact]
        public async Task Test_DeleteContact_ReturnsSuccess()
        {
            // Arrange
            var id = 1;

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var contatoExistente = new Contato
            {
                Id = id,
                Nome = "Nome Usuario",
                Telefone = "11987654321",
                DataCriacao = DateTime.Now.AddDays(-1)
            };

            dbContext.Contato.Add(contatoExistente);
            await dbContext.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync($"/excluir-contato/1");

            // Assert
            response.EnsureSuccessStatusCode();

            Console.WriteLine("Test_DeleteContact_ReturnsSuccess passou com sucesso!");
        }*/

        [Fact]
        public async Task Test_DeleteContact_ReturnsBadRequest()
        {
            var response = await _client.DeleteAsync("/api/contatos/excluir-contato/0");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Console.WriteLine("Test_DeleteContact_ReturnsBadRequest passou com sucesso!");
        }
    }
}