using Realms;

namespace KanbanBoardAPI.Models.Kanban;

public class KanbanBoardColumn
{
    public Guid KanbanBoard_Id { get; set; }

    public Guid Id { get; set; }
    public string Name { get; set; }
}
