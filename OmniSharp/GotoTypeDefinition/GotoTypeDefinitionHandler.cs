using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using OmniSharp.Parser;

namespace OmniSharp.GotoTypeDefinition
{
    public class GotoTypeDefinitionHandler
    {
        private readonly BufferParser _bufferParser;

        public GotoTypeDefinitionHandler(BufferParser bufferParser)
        {
            _bufferParser = bufferParser;
        }

        public GotoTypeDefinitionResponse GetGotoTypeDefinitionResponse(GotoTypeDefinitionRequest request)
        {
            var res = _bufferParser.ParsedContent(request.Buffer, request.FileName);
            var loc = new TextLocation(request.Line, request.Column);
            var resolveResult = ResolveAtLocation.Resolve(res.Compilation, res.UnresolvedFile, res.SyntaxTree, loc);

            var response = new GotoTypeDefinitionResponse();

            if (resolveResult != null)
            {
                var region = resolveResult.Type.GetDefinition().Region;

                response.FileName = region.FileName;
                response.Line = region.BeginLine;
                response.Column = region.BeginColumn;
            }

            return response;
        }
    }
}
