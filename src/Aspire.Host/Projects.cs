// Helper project marker types used by the Aspire Host builder when the Aspire SDK's
// generated `Projects` types are not present. These are intentionally minimal — they
// only exist so the generic `AddProject<T>` calls in `Program.cs` can compile.

using Aspire.Hosting;

namespace Projects
{
    // Implement IProjectMetadata to satisfy the AddProject<TProject> generic constraint.
    public sealed class Aspire_MinimalApi : IProjectMetadata
    {
        public string ProjectPath => "..\\Aspire.MinimalApi\\Aspire.MinimalApi.csproj";
    }

    public sealed class Aspire_ApiGateway : IProjectMetadata
    {
        public string ProjectPath => "..\\Aspire.ApiGateway\\Aspire.ApiGateway.csproj";
    }

    public sealed class Aspire_BlazorApp : IProjectMetadata
    {
        public string ProjectPath => "..\\Aspire.BlazorApp\\Aspire.BlazorApp.csproj";
    }
}
