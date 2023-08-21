using KanbanBoardAPI.Models.KanbanDto;

namespace KanbanBoardAPI.Services.DbServices;

public interface IKanbanRealmService
{
    public KanbanBoardDto CreateNewKanbanBoard(Guid userId, string name);
    public KanbanBoardDto GetKanbanBoard(Guid userId);
    public IEnumerable<KanbanBoardDto> GetKanbanBoards(Guid userId);
    public bool DeleteKanbanBoard(Guid userId, Guid id);

    public KanbanBoardColumnDto CreateNewKanbanColumn(Guid userId, Guid id, Guid KanbanBoard_Id, string name);
    public KanbanBoardColumnDto GetKanbanBoardColumn(Guid userId, Guid id);
    public IEnumerable<KanbanBoardColumnDto> GetKanbanBoardColumns(Guid userId, Guid kanbanBoard_Id);
    public bool DeleteKanbanBoardColumn(Guid userId, Guid id);
    public KanbanBoardColumnDto UpdateKanbanBoardColumn(Guid userId, Guid id, Guid KanbanBoard_Id, string name);


    public KanbanCardDto CreateKanbanCard(Guid userId, Guid id, Guid kanbanBoardColumn_Id, string name, string description, int index);
    public KanbanCardDto GetKanbanCard(Guid userId, Guid id);
    public IEnumerable<KanbanCardDto> GetKanbanCards(Guid userId, Guid columnId);
    public bool DeleteKanbanCard(Guid userId, Guid id);
    public KanbanCardDto UpdateKanbanBoardCard(Guid userId, Guid id, Guid kanbanBoardColumn_Id, string name, string description, int index);


    public KanbanCardRowDto CreateKanbanCardRow(Guid userId, Guid id, Guid KanbanCardId, string description, bool completed);
    public KanbanCardRowDto GetKanbanCardRow(Guid userId, Guid id);
    public IEnumerable<KanbanCardRowDto> GetKanbanCardRows(Guid userId, Guid cardId);
    public bool DeleteKanbanCardRow(Guid userId, Guid id);
    public KanbanCardRowDto UpdateKanbanBoardCardRow(Guid userId, Guid id, Guid KanbanCardId, string description, bool completed);

}
