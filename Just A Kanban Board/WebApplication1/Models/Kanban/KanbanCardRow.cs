using Realms;

namespace KanbanBoardAPI.Models.Kanban;

public class KanbanCardRow
{
    public Guid KanbanCard_Id { get; set; }

    public Guid Id { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
}
