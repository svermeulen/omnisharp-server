using Nancy;
using Nancy.ModelBinding;

namespace OmniSharp.ProjectManipulation.RenameFile
{
    public class RenameFileModule : NancyModule
    {
        public RenameFileModule(RenameFileHandler handler)
        {
            Post["/renamefile"] = x =>
                {
                    var req = this.Bind<RenameFileRequest>();
                    handler.RenameFile(req);
                    return Response.AsJson(true);
                };
        }
    }
}
