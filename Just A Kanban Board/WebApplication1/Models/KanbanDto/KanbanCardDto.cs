using KanbanBoardAPI.Models.KanbanRealm;

namespace KanbanBoardAPI.Models.KanbanDto;

public class KanbanCardDto
{
    public Guid KanbanBoardColumn_Id { get; set; }
    public Guid Id { get; set; }
    //Where in the list the card is rendered initially. 
    public int IndexId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<KanbanCardRowDto> CardRows { get; set; }
    
    public KanbanCardDto(KanbanCard card)
    {
        KanbanBoardColumn_Id = card.KanbanBoardColumn_Id;

        Id = card.Id;
        IndexId = card.IndexId;
        Name = card.Name;
        Description = card.Description;

        CardRows = new List<KanbanCardRowDto>();
    }
    public KanbanCardDto(KanbanCard card, IEnumerable<KanbanCardRow> rows)
    {
        KanbanBoardColumn_Id = card.Id;

        Id = card.Id;
        IndexId = card.IndexId;
        Name = card.Name;
        Description = card.Description;

        CardRows = new List<KanbanCardRowDto>();

        foreach (KanbanCardRow row in rows)
        {
            CardRows.Add(new KanbanCardRowDto(row));
        }
    }
    public KanbanCardDto(KanbanCard card, IEnumerable<KanbanCardRowDto> rows)
    {
        KanbanBoardColumn_Id = card.Id;

        Id = card.Id;
        IndexId = card.IndexId;
        Name = card.Name;
        Description = card.Description;

        CardRows = rows.ToList(); 
    }
}
