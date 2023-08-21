using KanbanBoardAPI.Models.KanbanRealm;

namespace KanbanBoardAPI.Models.KanbanDto;

public class KanbanCardRowDto
{
    public Guid KanbanCardRow_Id { get; set; }
    public Guid Id { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }

    public KanbanCardRowDto(KanbanCardRow row)
    {
        KanbanCardRow_Id = row.Id;

        Id = row.Id;
        Description = row.Description;
        Completed = row.Completed;
    }
}
