using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using VS4Mac.SamplesImporter.Controllers.Base;
using VS4Mac.SamplesImporter.Models;
using VS4Mac.SamplesImporter.Services;
using VS4Mac.SamplesImporter.Views.Base;

namespace VS4Mac.SamplesImporter.Controllers
{
    public class SamplesImporterController : IController
    {
        readonly ISamplesImporterView _view;
        readonly SampleImporterService _service;

        public SamplesImporterController(ISamplesImporterView view)
        {
            _view = view;
            _service = new SampleImporterService();

            view.SetController(this);
        }

        public Sample SelectedSample { get; set; }

        public List<Sample> LoadData()
        {
            return _service.GetSamples();
        }

        public List<Sample> FilterData(string filter)
        {
            return _service.GetSamples().Where(s => s.Name.Contains(filter)).ToList();
        }

        public async Task<string> DownloadSampleAsync()
        {
            try
            {
                string zipProjectPath = Path.Combine(UserProfile.Current.TempDir, $"{SelectedSample.Name}.zip");
                string projectPath = Path.Combine(UserProfile.Current.TempDir, SelectedSample.Name);

                if (!File.Exists(zipProjectPath))
                    await _service.DownloadSampleAsync(new Uri(SelectedSample.Url), zipProjectPath);

                if (!Directory.Exists(projectPath))
                {
                    Directory.CreateDirectory(projectPath);

                    ZipFile.ExtractToDirectory(zipProjectPath, projectPath);
                }

                return projectPath;
            }
            catch(Exception ex)
            {
                LoggingService.LogError(ex.Message);

                return string.Empty;
            }
        }

        public async Task OpenSolutionAsync(string solutionPath)
        {
            var solutions = Directory.GetFiles(solutionPath, "*.sln", SearchOption.AllDirectories);
            var solution = solutions.FirstOrDefault();

            if(!string.IsNullOrEmpty(solution))
                await IdeApp.Workspace.OpenWorkspaceItem(new FilePath(solution), true);
        }
    }
}