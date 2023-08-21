using Microsoft.Extensions.Configuration;
using System.Runtime;

namespace KanbanBoardAPI.Services;

public class WebAppConfig : IWebAppConfig
{
    public string KanbanRealmPath { get; private set; }
    public string GalleryRealmPath { get; private set; }
    public string UserRealmPath { get; private set; }

    public string BasePath { get; private set; }

    public int realmVersion { get; private set; }

    private readonly IWebHostEnvironment _environment;
    public WebAppConfig(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _environment = environment;

        IConfigurationSection dbSettings = configuration.GetSection("dbSettings"); 

        realmVersion = dbSettings.GetValue<int>("RealmVersion");

        BasePath = Path.Combine(_environment.ContentRootPath,
            dbSettings.GetValue<string>("RealmFolderName") ?? "realms");

        KanbanRealmPath =  Path.Combine(_environment.ContentRootPath, 
            dbSettings.GetValue<string>("RealmFolderName") ?? "realms", 
            dbSettings.GetValue<string>("KanbanRealmFileName") ?? "kanbanRealm.realm");

        GalleryRealmPath = Path.Combine(_environment.ContentRootPath,
            dbSettings.GetValue<string>("RealmFolderName") ?? "realms",
            dbSettings.GetValue<string>("GalleryRealmFileName") ?? "galleryRealmFileName.realm");

        UserRealmPath = Path.Combine(_environment.ContentRootPath,
            dbSettings.GetValue<string>("RealmFolderName") ?? "realms",
            dbSettings.GetValue<string>("UserRealmFileName") ?? "userRealmFileName.realm");
    }
}
