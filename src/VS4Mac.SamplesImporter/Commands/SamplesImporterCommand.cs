using MonoDevelop.Components.Commands;
using VS4Mac.SamplesImporter.Controllers;
using VS4Mac.SamplesImporter.Views.Base;

namespace VS4Mac.SamplesImporter.Commands
{
    public class SamplesImporterCommand: CommandHandler
    {
        protected override void Run()
        {
            var samplesImporterDialog = new SamplesImporterDialog();
            var samplesImporterController = new SamplesImporterController(samplesImporterDialog);
            samplesImporterDialog.Run(Xwt.MessageDialog.RootWindow);
        }
    }
}