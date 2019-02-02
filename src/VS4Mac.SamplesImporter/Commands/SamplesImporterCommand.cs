using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using VS4Mac.SamplesImporter.Controllers;
using VS4Mac.SamplesImporter.Views;

namespace VS4Mac.SamplesImporter.Commands
{
	public class SamplesImporterCommand: CommandHandler
    {
        protected override void Run()
        {
			try
			{
				var samplesImporterDialog = new SamplesImporterDialog();
				var samplesImporterController = new SamplesImporterController(samplesImporterDialog);
				samplesImporterDialog.Run(Xwt.MessageDialog.RootWindow);
			}
			catch(Exception ex)
			{
				MessageService.ShowError(ex.Message);
			}
		}
    }
}