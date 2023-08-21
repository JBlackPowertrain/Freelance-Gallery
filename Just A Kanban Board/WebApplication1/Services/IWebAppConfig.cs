namespace KanbanBoardAPI.Services;

public interface IWebAppConfig
{
    string KanbanRealmPath { get; }
    string GalleryRealmPath { get; }
    string UserRealmPath { get; }

    string BasePath { get; }

    int realmVersion { get; }
}
