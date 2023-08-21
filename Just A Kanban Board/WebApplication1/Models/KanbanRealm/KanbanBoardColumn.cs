using Realms;

namespace KanbanBoardAPI.Models.KanbanRealm;

public class KanbanBoardColumn : RealmObject
{
    public Guid User_Id { get; set; }
    public Guid KanbanBoard_Id { get; set; }

    [PrimaryKey]
    public Guid Id { get; set; }
    public string Name { get; set; }
}
