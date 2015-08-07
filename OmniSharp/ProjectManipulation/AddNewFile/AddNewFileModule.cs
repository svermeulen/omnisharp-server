using Nancy;
using Nancy.ModelBinding;

namespace OmniSharp.ProjectManipulation.AddNewFile
{
    public class AddNewFileModule : NancyModule
    {
        public AddNewFileModule(AddNewFileHandler handler)
        {
            Post["/addnewfile"] = x =>
                {
                    var req = this.Bind<AddNewFileRequest>();
                    handler.AddNewFile(req);
                    return Response.AsJson(true);
                };
        }
    }
}
