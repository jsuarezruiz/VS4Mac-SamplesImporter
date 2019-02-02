using MonoDevelop.Components;
using MonoDevelop.Ide.Gui.Dialogs;
using VS4Mac.SamplesImporter.Controls;

namespace VS4Mac.SamplesImporter.Views
{
	public class SamplesImporterSettingsPanel : OptionsPanel
	{
		SamplesImporterSettingsWidget _widget;

		public override void ApplyChanges()
		{
			_widget.ApplyChanges();
		}

		public override Control CreatePanelWidget()
		{
			_widget = new SamplesImporterSettingsWidget();

			return new XwtControl(_widget);
		}
	}
}