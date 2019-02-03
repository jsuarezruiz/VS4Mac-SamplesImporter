using System;
using System.Runtime.InteropServices;
using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
    "SamplesImporter",
    Namespace = "MonoDevelop",
    Version = "0.1", 
    Category = "IDE extensions"
)]

[assembly: AddinName("VS4Mac.SamplesImporter")]
[assembly: AddinCategory("IDE extensions")]
[assembly: AddinDescription("Samples Importer is a Visual Studio for macOS addin to explorer and directly open Xamarin samples from the IDE.")]
[assembly: AddinAuthor("Javier Suárez Ruiz")]
[assembly: AddinUrl("https://github.com/jsuarezruiz/VS4Mac-SamplesImporter")]

[assembly: CLSCompliant(false)]
[assembly: ComVisible(false)]
