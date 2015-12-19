using System.IO;
using OmniSharp.Solution;

namespace OmniSharp.Build
{
    public class BuildCommandBuilder
    {
        readonly ISolution _solution;

        public BuildCommandBuilder(ISolution solution)
        {
            _solution = solution;
        }

        public string GetCommand(bool useDevenv)
        {
            return GetExecutable(useDevenv).ApplyPathReplacementsForClient() + " " + GetArguments(useDevenv);
        }

        public string GetExecutable(bool useDevenv)
        {
            if (useDevenv)
            {
                return "\"C:/Program Files (x86)/Microsoft Visual Studio 12.0/Common7/IDE/devenv.com\"";
            }

            return "\"C:/Program Files (x86)/MSBuild/12.0/Bin/MSBuild.exe\"";
        }

        public string GetArguments(bool useDevenv)
        {
            if (useDevenv)
            {
                return _solution.FileName + " /build";
            }

            return "/m /nologo /v:q /property:GenerateFullPaths=true \"" + _solution.FileName + "\"";
        }
    }
}
