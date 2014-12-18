using Nancy;
using Nancy.ModelBinding;

namespace OmniSharp.GotoTypeDefinition
{
    public class GotoTypeDefinitionModule : NancyModule
    {
        public GotoTypeDefinitionModule(GotoTypeDefinitionHandler gotoDefinitionHandler)
        {
            Post["/gototypedefinition"] = x =>
                {
                    var req = this.Bind<GotoTypeDefinitionRequest>();
                    var res = gotoDefinitionHandler.GetGotoTypeDefinitionResponse(req);
                    return Response.AsJson(res);
                };
        }
    }
}
