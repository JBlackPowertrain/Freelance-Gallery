using FreelanceGalleryProjectTests.Helpers;
using KanbanBoardAPI.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace FreelanceGalleryProjectTests;

public class ConfigTests
{
    private Mock<IWebAppConfig> _webbAppConfigMock;
    private string GalleryRealmPath;
    private string KanbanRealmPath;
    private string UserRealmPath;
    private string baseDir; 

    [SetUp]
    public void Setup()
    {
        baseDir = AppDomain.CurrentDomain.BaseDirectory; 

        var jsonConfig = IConfigBuilder.InitConfig();
        IConfigurationSection dbSettings = jsonConfig.GetSection("dbSettings");

        KanbanRealmPath = Path.Combine(baseDir,
            dbSettings.GetValue<string>("RealmFolderName") ?? "realms",
            dbSettings.GetValue<string>("KanbanRealmFileName") ?? "kanbanRealm.realm");

        GalleryRealmPath = Path.Combine(baseDir,
            dbSettings.GetValue<string>("RealmFolderName") ?? "realms",
            dbSettings.GetValue<string>("GalleryRealmFileName") ?? "galleryRealmFileName.realm");

        UserRealmPath = Path.Combine(baseDir,
            dbSettings.GetValue<string>("RealmFolderName") ?? "realms",
            dbSettings.GetValue<string>("UserRealmFileName") ?? "userRealmFileName.realm");

        _webbAppConfigMock = new Mock<IWebAppConfig>();
    }

    [Test]
    public void RealmAppSettingsConfigTest()
    {
        Assert.IsNotNull(KanbanRealmPath);
        Assert.IsNotNull(GalleryRealmPath);
        Assert.IsNotNull(UserRealmPath);

        Assert.IsFalse(string.IsNullOrWhiteSpace(KanbanRealmPath));
        Assert.IsFalse(string.IsNullOrWhiteSpace(GalleryRealmPath));
        Assert.IsFalse(string.IsNullOrWhiteSpace(UserRealmPath));
    }

    [Test]
    public void webAppConfigTest()
    {
        _webbAppConfigMock.Setup(x => x.realmVersion).Returns(99);
        int result1 = _webbAppConfigMock.Object.realmVersion; 
        _webbAppConfigMock.Verify(x => x.realmVersion, Times.Once);
        _webbAppConfigMock.VerifyGet(x => x.realmVersion);

        _webbAppConfigMock.Setup(x => x.KanbanRealmPath).Returns(KanbanRealmPath);
        string result2 = _webbAppConfigMock.Object.KanbanRealmPath;
        _webbAppConfigMock.Verify(x => x.KanbanRealmPath, Times.Once);
        _webbAppConfigMock.VerifyGet(x => x.KanbanRealmPath);
        Assert.IsTrue(KanbanRealmPath == result2);

        _webbAppConfigMock.Setup(x => x.GalleryRealmPath).Returns(GalleryRealmPath);
        string result3 = _webbAppConfigMock.Object.GalleryRealmPath;
        _webbAppConfigMock.Verify(x => x.GalleryRealmPath, Times.Once);
        _webbAppConfigMock.VerifyGet(x => x.GalleryRealmPath);
        Assert.IsTrue(GalleryRealmPath == result3);

        _webbAppConfigMock.Setup(x => x.UserRealmPath).Returns(UserRealmPath);
        string result4 = _webbAppConfigMock.Object.UserRealmPath;
        _webbAppConfigMock.Verify(x => x.UserRealmPath, Times.Once);
        _webbAppConfigMock.VerifyGet(x => x.UserRealmPath);
        Assert.IsTrue(UserRealmPath == result4);

        Assert.Pass(); 
    }
}