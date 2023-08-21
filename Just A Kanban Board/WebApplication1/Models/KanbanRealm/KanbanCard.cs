
using Realms;

namespace KanbanBoardAPI.Models.KanbanRealm;

public class KanbanCard : RealmObject
{
    public Guid User_Id { get; set; }
    public Guid KanbanBoardColumn_Id { get; set; }

    [PrimaryKey]
    public Guid Id { get; set; }
    //Where in the list the card is rendered initially. 
    public int IndexId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
