# XorTagCore

## Project Structure

- `XorTag/` - Main ASP.NET Core web application (runs on http://localhost:5007)
- `XorTag.UnitTests/` - Unit tests (NUnit)
- `XorTag.AcceptanceTests/` - Acceptance tests using WebApplicationFactory (NUnit)

## Build and Test Commands

```bash
# Build the solution
dotnet build

# Run unit tests
dotnet test XorTag.UnitTests

# Run acceptance tests
dotnet test XorTag.AcceptanceTests

# Run the application
dotnet run --project XorTag
```

## Upgrading Magick.NET-Q8-AnyCPU

This dependency is used for image generation (map rendering). It is referenced in all three projects.

### Steps to Upgrade

1. **Check for updates:**
   ```bash
   dotnet list XorTag package --outdated
   ```

2. **Update the version in all three csproj files:**
   - `XorTag/XorTag.csproj`
   - `XorTag.UnitTests/XorTag.UnitTests.csproj`
   - `XorTag.AcceptanceTests/XorTag.AcceptanceTests.csproj`

3. **Build the solution:**
   ```bash
   dotnet build
   ```

4. **Run all tests:**
   ```bash
   dotnet test XorTag.UnitTests
   dotnet test XorTag.AcceptanceTests
   ```

5. **Manual verification - test the /map endpoint:**
   ```bash
   # Start the application
   dotnet run --project XorTag

   # In another terminal, test the map endpoint returns HTTP 200 and image/png
   curl -s -I http://localhost:5007/map | grep -E "HTTP|Content-Type"
   # Should show: HTTP/1.1 200 OK
   #              Content-Type: image/png
   ```

