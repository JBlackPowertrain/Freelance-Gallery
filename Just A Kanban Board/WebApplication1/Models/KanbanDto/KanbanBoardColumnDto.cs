using KanbanBoardAPI.Models.KanbanRealm;

namespace KanbanBoardAPI.Models.KanbanDto;

public class KanbanBoardColumnDto
{
    public Guid KanbanBoard_Id { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IList<KanbanCardDto> Cards { get; set; }

    public KanbanBoardColumnDto(KanbanBoardColumn column)
    {
        KanbanBoard_Id = column.Id;

        Id = column.Id;
        Name = column.Name;

        Cards = new List<KanbanCardDto>();
    }

    public KanbanBoardColumnDto(KanbanBoardColumn column, IEnumerable<KanbanCard> cards)
    {
        KanbanBoard_Id = column.Id;

        Id = column.Id;
        Name = column.Name;

        Cards = new List<KanbanCardDto>(); 

        foreach (KanbanCard card in cards)
        {
            Cards.Add(new KanbanCardDto(card));
        }
    }

    public KanbanBoardColumnDto(KanbanBoardColumn column, IEnumerable<KanbanCardDto> cards)
    {
        KanbanBoard_Id = column.Id;

        Id = column.Id;
        Name = column.Name;

        Cards = cards.ToList();
    }
}
