using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using OmniSharp.Parser;
using OmniSharp.Solution;
using OmniSharp.Common;

namespace OmniSharp.ProjectManipulation.AddToProject
{
    public class AddToProjectHandler
    {
        private readonly ISolution _solution;
        private readonly XNamespace _msBuildNameSpace = "http://schemas.microsoft.com/developer/msbuild/2003";
        private readonly IFileSystem _fileSystem;

        public AddToProjectHandler(ISolution solution, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _solution = solution;
        }

        public void AddToProject(AddToProjectRequest request)
        {
            if (request.FileName == null || !request.FileName.EndsWith(".cs"))
            {
                return;
            }

            var relativeProject = _solution.ProjectContainingFile(request.FileName);

            if (relativeProject == null || relativeProject is OrphanProject)
            {
                throw new ProjectNotFoundException(string.Format("Unable to find project relative to file {0}", request.FileName));
            }

            AddToProject(relativeProject, request.FileName);
        }

        static string CaseInsensitiveReplace(string originalString, string oldValue, string newValue, StringComparison comparisonType)
        {
            int startIndex = 0;
            while (true)
            {
                startIndex = originalString.IndexOf(oldValue, startIndex, comparisonType);
                if (startIndex == -1)
                    break;

                originalString = originalString.Substring(0, startIndex) + newValue + originalString.Substring(startIndex + oldValue.Length);

                startIndex += newValue.Length;
            }

            return originalString;
        }

        public void AddToProject(IProject relativeProject, string fileName)
        {
            var project = relativeProject.AsXml();

            var requestFile = Path.GetFullPath(fileName);
            var projectPath = Path.GetFullPath(relativeProject.FileName);
            var projectDirectory = _fileSystem.GetDirectoryName(projectPath);
            var relativeFileName = CaseInsensitiveReplace(requestFile, projectDirectory, "", StringComparison.OrdinalIgnoreCase).ForceWindowsPathSeparator().Substring(1);

            var compilationNodes = project.Element(_msBuildNameSpace + "Project")
                                          .Elements(_msBuildNameSpace + "ItemGroup")
                                          .Elements(_msBuildNameSpace + "Compile").ToList();

            var fileAlreadyInProject = compilationNodes.Any(n => n.Attribute("Include").Value.Equals(relativeFileName, StringComparison.InvariantCultureIgnoreCase));

            if (!fileAlreadyInProject)
            {
                var compilationNodeParent = compilationNodes.First().Parent;

                var newFileElement = new XElement(_msBuildNameSpace + "Compile", new XAttribute("Include", relativeFileName));

                compilationNodeParent.Add(newFileElement);

                relativeProject.Save(project);
            }

            var parser = new BufferParser(_solution);
            parser.ParsedContent(relativeProject, File.ReadAllText(requestFile), requestFile);
        }
    }
}
