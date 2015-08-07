using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using OmniSharp.Solution;

namespace OmniSharp.ProjectManipulation.RemoveFromProject
{
    public class RemoveFromProjectHandler
    {
        readonly ISolution _solution;

        public RemoveFromProjectHandler(ISolution solution)
        {
            _solution = solution;
        }

        public void RemoveFromProject(RemoveFromProjectRequest request)
        {
            var relativeProject = _solution.ProjectContainingFile(request.FileName);

            if (relativeProject == null || relativeProject is OrphanProject)
            {
                throw new ProjectNotFoundException(string.Format("Unable to find project relative to file {0}", request.FileName));
            }

            RemoveFromProject(relativeProject, request.FileName);
        }

        public void RemoveFromProject(IProject relativeProject, string fileName)
        {
            var project = relativeProject.AsXml();

            var fileNameFull = Path.GetFullPath(fileName);
            var projectPathFull = Path.GetFullPath(relativeProject.FileName);

            var separator = projectPathFull.Contains ("/") ? "/" : "\\";
            var relativeFileName = fileNameFull.Replace(projectPathFull.Substring(0, projectPathFull.LastIndexOf(separator) + 1), "")
                .Replace("/", @"\");

            var fileNode = project.CompilationNodes()
                .FirstOrDefault(n => n.Attribute("Include")
                                .Value.Equals(relativeFileName, StringComparison.InvariantCultureIgnoreCase));

            if (fileNode != null)
            {
                RemoveFileFromProject(relativeProject, fileNameFull);

                project.CompilationNodes().Where(n => n.Attribute("Include").Value.Equals(relativeFileName, StringComparison.InvariantCultureIgnoreCase)).Remove();

                var nodes = project.CompilationNodes();

                if (!nodes.Any())
                {
                    project.ItemGroupNodes().Where(n => !n.Nodes().Any()).Remove();
                }

                relativeProject.Save(project);
            }
        }

        private void RemoveFileFromProject(IProject project, string filename)
        {
            project.Files.Remove(project.Files.First(f => f.FileName == filename));
        }
    }
}
