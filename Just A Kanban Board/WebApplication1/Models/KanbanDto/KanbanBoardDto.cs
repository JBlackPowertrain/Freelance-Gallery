using KanbanBoardAPI.Models.KanbanRealm; 

namespace KanbanBoardAPI.Models.KanbanDto;

public class KanbanBoardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IList<KanbanBoardColumnDto> Columns { get; set; }

    public KanbanBoardDto(KanbanBoard board)
    {
        Id = board.Id;
        Name = board.Name;

        Columns = new List<KanbanBoardColumnDto>();
    }

    public KanbanBoardDto(KanbanBoard board, IEnumerable<KanbanBoardColumn> columns)
    {
        Id = board.Id;
        Name = board.Name;

        Columns = new List<KanbanBoardColumnDto>();
        foreach (KanbanBoardColumn column in columns)
        {
            Columns.Add(new KanbanBoardColumnDto(column));
        }
    }

    public KanbanBoardDto(KanbanBoard board, IEnumerable<KanbanBoardColumnDto> columns)
    {
        Id = board.Id;
        Name = board.Name;

        Columns = columns.ToList();
    }
}
