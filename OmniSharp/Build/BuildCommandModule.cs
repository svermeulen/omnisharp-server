using Nancy;
using OmniSharp.Solution;
using Nancy.ModelBinding;

namespace OmniSharp.Build
{
    public class BuildCommandModule : NancyModule
    {
        public BuildCommandModule(BuildCommandBuilder commandBuilder)
        {
            Post["/buildcommand"] = x =>
            {
                var req = this.Bind<BuildRequest>();
                return Response.AsText(commandBuilder.GetCommand(req.UseDevenv));
            };
        }
    }
}
