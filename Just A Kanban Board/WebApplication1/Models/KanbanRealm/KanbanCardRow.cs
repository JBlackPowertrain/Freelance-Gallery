using Realms;

namespace KanbanBoardAPI.Models.KanbanRealm;

public class KanbanCardRow : RealmObject
{
    public Guid User_Id { get; set; }
    public Guid KanbanCard_Id { get; set; }

    [PrimaryKey]
    public Guid Id { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
}
