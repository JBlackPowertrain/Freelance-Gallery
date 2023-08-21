namespace KanbanBoardAPI.Models;

//https://github.com/realm/realm-dotnet/issues/959
//When moving to SQL, purge realm and replace w/ this base class. Purge User_Id from all objects
public abstract class BaseModel
{
    public Guid User_Id { get; set; }
}
