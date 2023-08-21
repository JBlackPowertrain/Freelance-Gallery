using Realms;

namespace KanbanBoardAPI.Models.KanbanRealm;

public class KanbanBoard : RealmObject
{
    public Guid User_Id { get; set; }

    [PrimaryKey]
    public Guid Id { get; set; }
    public string Name { get; set; }
}
