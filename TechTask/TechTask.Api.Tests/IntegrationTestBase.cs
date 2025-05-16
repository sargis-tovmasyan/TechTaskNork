using Microsoft.AspNetCore.Mvc.Testing;

namespace TechTask.Api.Tests;

public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;

    public IntegrationTestBase(WebApplicationFactory<Program> factory)
    {
        Client = factory.CreateClient();
    }
}