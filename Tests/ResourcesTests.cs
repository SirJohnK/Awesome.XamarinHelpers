using Awesome.XamarinHelpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms;

namespace Tests
{
    [TestClass]
    public class ResourcesTests
    {
        private readonly ResourceDictionary resources;

        public ResourcesTests()
        {
            //Init
            MockForms.Init();

            //Load resources
            resources = new Resources();
        }

        [TestMethod]
        public void FindResource_ShouldReturnCorrectTypeAndValue_SimpleResource()
        {
            //Find simple resource
            resources.FindResource<Color>("FirstColor", Color.Default).Should().Be(Color.Red);

            MockForms.Init(Device.Android);
            var fontSize = resources.FindResource<double>("DefaultFontSize", 0);

            var txt1 = resources.FindResource<string>("IntroText", "Hello Nobody!");

            MockForms.Init(Device.iOS);
            var txt2 = resources.FindResource<string>("IntroText", "Hello Nobody!");
        }

        [TestMethod]
        public void FindResource_ShouldReturnCorrectTypeAndValue_BasedOnIdiom()
        {
            //Find resorce for Desktop idiom
            MockForms.Init(idiom: TargetIdiom.Desktop);
            resources.FindResource<Color>("IdiomColor", Color.Default).Should().Be(Color.Red);

            //Find resorce for Desktop idiom
            MockForms.Init(idiom: TargetIdiom.Phone);
            resources.FindResource<Color>("IdiomColor", Color.Default).Should().Be(Color.Green);

            //Find resorce for Desktop idiom
            MockForms.Init(idiom: TargetIdiom.Tablet);
            resources.FindResource<Color>("IdiomColor", Color.Default).Should().Be(Color.Blue);

            //Find resorce for Unsupported idiom
            MockForms.Init(Device.iOS, TargetIdiom.Unsupported);
            resources.FindResource<Color>("PlatformIdiomColor", Color.Default).Should().Be(Color.Yellow);
        }

        [TestMethod]
        public void FindResource_ShouldReturnCorrectTypeAndValue_WithSimpleDynamicResource()
        {
            //Init
            MockForms.Init(Device.Android, TargetIdiom.Phone);

            var primaryColor = resources.FindResource<Color>("PrimaryColor", Color.Default);
            var platformColor = resources.FindResource<Color>("PlatformColor", Color.Default);
            var idomColor = resources.FindResource<Color>("IdiomColor", Color.Default);
            var runtimeColor = resources.FindResource<Color>("RuntimeColor", Color.Default);
            var platformIdiomColor = resources.FindResource<Color>("PlatformIdiomColor", Color.Default);
        }

        [TestMethod]
        public void FindResource_ShouldReturnCorrectTypeAndValue_WithSimpleDynamicResource2()
        {
            //Init
            MockForms.Init(Device.iOS, TargetIdiom.Phone);

            var primaryColor = resources.FindResource<Color>("PrimaryColor", Color.Default);
            var platformColor = resources.FindResource<Color>("PlatformColor", Color.Default);
            var idomColor = resources.FindResource<Color>("IdiomColor", Color.Default);
            var runtimeColor = resources.FindResource<Color>("RuntimeColor", Color.Default);
            var platformIdiomColor = resources.FindResource<Color>("PlatformIdiomColor", Color.Default);
        }

        [TestMethod]
        public void FindResource_ShouldReturnCorrectTypeAndValue_WithSimpleDynamicResource3()
        {
            //Init
            MockForms.Init(Device.Android, TargetIdiom.Tablet);

            var primaryColor = resources.FindResource<Color>("PrimaryColor", Color.Default);
            var platformColor = resources.FindResource<Color>("PlatformColor", Color.Default);
            var idomColor = resources.FindResource<Color>("IdiomColor", Color.Default);
            var runtimeColor = resources.FindResource<Color>("RuntimeColor", Color.Default);
            var platformIdiomColor = resources.FindResource<Color>("PlatformIdiomColor", Color.Default);
        }

        [TestMethod]
        public void FindResource_ShouldReturnCorrectTypeAndValue_WithSimpleDynamicResource4()
        {
            //Init
            MockForms.Init(Device.iOS, TargetIdiom.Tablet);

            var primaryColor = resources.FindResource<Color>("PrimaryColor", Color.Default);
            var platformColor = resources.FindResource<Color>("PlatformColor", Color.Default);
            var idomColor = resources.FindResource<Color>("IdiomColor", Color.Default);
            var runtimeColor = resources.FindResource<Color>("RuntimeColor", Color.Default);
            var platformIdiomColor = resources.FindResource<Color>("PlatformIdiomColor", Color.Default);
        }
    }
}