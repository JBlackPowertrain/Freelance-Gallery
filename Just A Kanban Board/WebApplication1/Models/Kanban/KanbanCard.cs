
using Realms;

namespace KanbanBoardAPI.Models.Kanban;

public class KanbanCard
{
    public Guid KanbanBoardColumn_Id { get; set; }

    public Guid Id { get; set; }
    //Where in the list the card is rendered initially. 
    public int IndexId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
