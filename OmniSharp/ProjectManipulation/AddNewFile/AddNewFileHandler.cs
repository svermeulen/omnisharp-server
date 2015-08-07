using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using OmniSharp.Solution;
using OmniSharp.Common;
using OmniSharp.ProjectManipulation.RemoveFromProject;
using OmniSharp.ProjectManipulation.AddToProject;

namespace OmniSharp.ProjectManipulation.AddNewFile
{
    public class AddNewFileHandler
    {
        private readonly ISolution _solution;
        private readonly IFileSystem _fileSystem;

        public AddNewFileHandler(ISolution solution, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _solution = solution;
        }

        public void AddNewFile(AddNewFileRequest request)
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

            File.WriteAllText(request.NewFileName, "");

            var addHandler = new AddToProjectHandler(_solution, _fileSystem);
            addHandler.AddToProject(relativeProject, request.NewFileName);
        }
    }
}
