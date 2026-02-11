using Microsoft.AspNetCore.Mvc.Testing;
using XorTag.Controllers;

namespace XorTag.AcceptanceTests;

public static class TestHelpers
{
    public static WebApplicationFactory<MapController> CreateTestFactory()
    {
        var factory = new WebApplicationFactory<MapController>();
        return factory;
    }
}
