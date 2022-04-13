using Funq;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;

[assembly: FunctionsStartup(typeof(WildcardRouting.Startup))]
namespace WildcardRouting;
public class Startup : FunctionsStartup
{
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder configBuilder) {
        var context = configBuilder.GetContext();
        if (File.Exists(Path.Combine(context.ApplicationRootPath, "local.settings.json"))) {
            configBuilder.ConfigurationBuilder.AddJsonFile(Path.Combine(context.ApplicationRootPath, "local.settings.json"), optional: true, reloadOnChange: false);
        }
        configBuilder.ConfigurationBuilder.AddEnvironmentVariables();
    }

    public override void Configure(IFunctionsHostBuilder builder) { new AppHost(builder.GetContext().Configuration).Init(); }
}

[Route("/cat/facts","GET")]
public class CatFactRequest : IReturn<object>, IGet { }

[Route("/dog/random","GET")]
public class DogRandomRequest : IReturn<object>, IGet { }

[Route("/picsum/{Id}","GET")]
public class PicsumRequest: IReturn<object>, IGet { 
    public int Id { get; set; }
}

[Route("/nasa", "GET")]
public class NasaRequest : IReturn<object>, IGet { }


public class WildServices : Service
{
    private readonly IAppSettings _appSetting;
    public WildServices(IAppSettings appSetting)
    {
        _appSetting = appSetting;
    }
    public object Get(CatFactRequest req) => "https://cat-fact.herokuapp.com/facts".GetJsonFromUrl();
    public object Get(DogRandomRequest req) => "https://dog.ceo/api/breeds/image/random".GetJsonFromUrl();
    public object Get(PicsumRequest req) => $"https://picsum.photos/id/{req.Id}/info".GetJsonFromUrl();

    public object Get(NasaRequest req) => $"https://api.nasa.gov/planetary/apod?api_key={_appSetting.Get<string>("Keys:Nasa")}".GetJsonFromUrl();
        //N0tdchC2gS1ZagKlvrAE3KakZH0erpLYAsCwq89Q


}
public class AppHost: AppHostBase
{
   
    public AppHost(IConfiguration configuration) : base( "Wildcard", typeof(WildServices).Assembly) { 

        AppSettings = new NetCoreAppSettings(configuration);
    } 
    public override void Configure(Container container)
    {
        base.SetConfig(new HostConfig
        {
            DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false)
        });
      
    }
}
