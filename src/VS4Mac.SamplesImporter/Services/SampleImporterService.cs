using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using VS4Mac.SamplesImporter.Models;

namespace VS4Mac.SamplesImporter.Services
{
    public class SampleImporterService
    {
        public List<Sample> GetSamples()
        {
            // TODO: In next versions, the samples will be obtained from GitHub, from the repositories of samples of 
            // Xamarin.Android, Xamarin.iOS, Xamarin.Forms and Xamarin.Mac. 
            // This list is a quick prototype to advance in the UI and the rest of the addin.
            return new List<Sample>
            {
                new Sample { Name = "BikeSharing", Screenshot = "Images/bikesharing.png", Description = "BikeSharing360 is a fictitious example of a smart bike sharing system with 10,000 bikes distributed in 650 stations located throughout New York City and Seattle. Their vision is to provide a modern and personalized experience to riders and to run their business with intelligence. Was a demo in the Connect(); 2016.", Platforms = new List<string> { "Android", "iOS", "UWP" }, Tags = new List<string> { "Xamarin.Forms" }, Url = "https://github.com/Microsoft/BikeSharing360_MobileApps/archive/master.zip" },
                new Sample { Name = "Bikr", Screenshot = "Images/bikr.png", Description = "A bike activity App. Sample to show Microcharts usage.", Platforms = new List<string> { "Android", "iOS", "UWP" }, Tags = new List<string> { "Xamarin.Forms", "SkiaSharp" }, Url = "https://github.com/aloisdeniel/Microcharts.Samples/archive/master.zip" },
                new Sample { Name = "BindableLayoutPlayground", Screenshot = "Images/bindablelayout.png", Description = "A Xamarin.Forms sample to show how to use Bindable Layout.", Platforms = new List<string> { "Android", "iOS", "UWP" }, Tags = new List<string> { "Xamarin.Forms", "Bindable Layout" }, Url = "https://github.com/jsuarezruiz/BindableLayoutPlayground/archive/master.zip" },
                new Sample { Name = "ConferenceVision", Screenshot = "Images/conferencevision.png", Description = "Sample Xamarin.Forms 3.0 Phone App showed in Microsoft Build 2018. Use your camera during a conference to capture your experience. Let Vision do the heavy lifting of identifying known products and scan your images to text for easier searching.", Platforms = new List<string> { "Android", "iOS", "UWP" }, Tags = new List<string> { "Xamarin.Forms", "FlexLayout", "Camera" }, Url = "https://github.com/Microsoft/ConferenceVision/archive/master.zip" },
                new Sample { Name = "Evolve", Screenshot = "Images/evolve.png", Description = "Xamarin Evolve 2016 Mobile App. This app is around 15,000 lines of code. The iOS version contains 93% shared code, the Android version contains 90% shared code, the UWP has 99% shared code, and our Azure backend contains 23% shared code with the clients!", Platforms = new List<string> { "Android", "iOS", "UWP" }, Tags = new List<string> { "Xamarin.Forms" }, Url = "https://github.com/xamarinhq/app-conference/archive/master.zip" },
                new Sample { Name = "Facebook", Screenshot = "Images/facebook.png", Description = "A Xamarin.Forms version of the Facebook App.", Platforms = new List<string> { "Android", "iOS", "UWP" }, Tags = new List<string> { "Xamarin.Forms" }, Url = "https://github.com/XAM-Consulting/FacebookForms/archive/master.zip" },
                new Sample { Name = "KickassUI.FancyTutorial", Screenshot = "Images/fancytutorial.png", Description = "A simple (but beautiful) Xamarin.Forms tutorial screen.", Platforms = new List<string> { "Android", "iOS" }, Tags = new List<string> { "Xamarin.Forms", "Carousel" }, Url = "https://github.com/sthewissen/KickassUI.FancyTutorial/archive/master.zip" },
                new Sample { Name = "KickassUI.ParallaxCarousel", Screenshot = "Images/parallaxcarousel.png", Description = "A Xamarin.Forms Carousel using a nice parallax effect.", Platforms = new List<string> { "Android", "iOS" }, Tags = new List<string> { "Xamarin.Forms", "Parallax" }, Url = "https://github.com/sthewissen/KickassUI.ParallaxCarousel/archive/master.zip" },
                new Sample { Name = "KickassUI.Runkeeper", Screenshot = "Images/runkeeper.png", Description = "A Xamarin.Forms version of the Runkeeper App.",  Platforms = new List<string> { "Android", "iOS" }, Tags = new List<string> { "Xamarin.Forms", "Carousel", "Map" }, Url = "https://github.com/sthewissen/KickassUI.Runkeeper/archive/master.zip" },
                new Sample { Name = "KickassUI.Spotify", Screenshot = "Images/spotify.png", Description = "A Xamarin.Forms version of the Spotify App.",  Platforms = new List<string> { "Android", "iOS" }, Tags = new List<string> { "Xamarin.Forms", "Tabs" }, Url = "https://github.com/sthewissen/KickassUI.Spotify/archive/master.zip" },
                new Sample { Name = "KickassUI.Twitter", Screenshot = "Images/twitter.png", Description = "A Xamarin.Forms version of the Twitter App.", Platforms = new List<string> { "Android", "iOS" }, Tags = new List<string> { "Xamarin.Forms", "Floating Button" }, Url = "https://github.com/sthewissen/KickassUI.Twitter/archive/master.zip" },
                new Sample { Name = "My Trip Countdown", Screenshot = "Images/mytripcountdown.png", Description = "A Xamarin.Forms timer until your next vacation.", Platforms = new List<string> { "Android", "iOS" }, Tags = new List<string> { "Xamarin.Forms", "SkiaSharp" }, Url = "https://github.com/jsuarezruiz/MyTripCountdown/archive/master.zip" },
                new Sample { Name = "Movies", Screenshot = "Images/movies.png", Description = "Movies is a Xamarin.Forms application that makes use of The Movie Database (TMDb) API.", Platforms = new List<string> { "Android", "iOS", "UWP", "GTK", "WPF" }, Tags = new List<string> { "Xamarin.Forms" }, Url = "https://github.com/jsuarezruiz/xamarin-forms-gtk-movies-sample/archive/master.zip"  },
                new Sample { Name = "Posy", Screenshot = "Images/posy.png", Description = "A simple but good looking Xamarin.Forms UI screen.", Platforms = new List<string> { "Android", "iOS" }, Tags = new List<string> { "Xamarin.Forms", "Animation" }, Url = "https://github.com/sthewissen/KickassUI.InSpace/archive/master.zip" },
                new Sample { Name = "Pulse Music", Screenshot = "images/pulsemusic.png", Description = "A Xamarin.Forms music player sample.", Platforms = new List<string> { "Android", "iOS" }, Tags = new List<string> { "Xamarin.Forms" }, Url = "https://github.com/jsuarezruiz/PulseMusic/archive/master.zip" },
                new Sample { Name = "Restaurant App", Screenshot = "Images/restaurantapp.png", Description = "Restaurant App is a sample open source application, powered by ASP.NET Core 2.1 and Xamarin.Forms 3.0. It incorporates material design and provides us with an overview of how to build mobile and web applications with a clean architecture and write testable and clean code.", Platforms = new List<string> { "Android", "iOS" }, Tags = new List<string> { "Xamarin.Forms", "Material" }, Url = "https://github.com/Jurabek/Restaurant-App/archive/develop.zip" },
                new Sample { Name = "SmartHotel 360", Screenshot = "Images/smarthotel.png", Description = "Travelers are always on the go, so SmartHotel360 offers a beautiful fully-native cross-device mobile app for guests and business travelers built with Xamarin.", Platforms = new List<string> { "Android", "iOS", "UWP" }, Tags = new List<string> { "Xamarin.Forms", "SkiaSharp", "Map" }, Url = "https://github.com/Microsoft/SmartHotel360-Mobile/archive/master.zip" },
                new Sample { Name = "Tailwind Traders", Screenshot = "Images/taildwindtraders.png", Description = "A fictitious retail company showcasing the future of intelligent mobile application experiences.", Platforms = new List<string> { "Android", "iOS" }, Tags = new List<string> { "Xamarin.Forms", "Shell" }, Url = "https://github.com/Microsoft/TailwindTraders-Mobile/archive/develop.zip" },
                new Sample { Name = "Timeline", Screenshot = "Images/timeline.png", Description = "A timeline of activities. This is useful for things like transportation schedules or class times.", Platforms = new List<string> { "Android", "iOS" }, Tags = new List<string> { "Xamarin.Forms", "ListView" }, Url = "https://github.com/kphillpotts/XamarinFormsLayoutChallenges/archive/master.zip" },
                new Sample { Name = "Xamarin.Netflix", Screenshot = "Images/netflix.png", Description = "A Xamarin.Forms version of the Netflix App.", Platforms = new List<string> { "Android", "iOS" }, Tags = new List<string> { "Xamarin.Forms", "Custom Renderer", "Parallax" }, Url = "https://github.com/jsuarezruiz/xamarin-forms-netflix-sample/archive/master.zip" },
                new Sample { Name = "WalkthroughApp", Screenshot = "Images/walkthroughapp.png", Description = "WalkthroughApp is a Xamarin.Forms application that makes use of CarouselView and Lottie, to demonstrate the possibilities of creating a fluid and powerful Walkthrough without requiring Custom Renderers.", Platforms = new List<string> { "Android", "iOS" }, Tags = new List<string> { "Xamarin.Forms", "Lottie", "Animation" }, Url = "https://github.com/jsuarezruiz/xamarin-forms-walkthrough/archive/master.zip" }
            };
        }

        public async Task DownloadSampleAsync(Uri requestUri, string filename)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));
               
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
                {
                    using (
                        Stream contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(),
                        stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 1024, true))
                    {
                        await contentStream.CopyToAsync(stream);
                    }
                }
            }
        }
    }
}