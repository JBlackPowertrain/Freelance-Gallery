#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

using Realms;
using Realms.Exceptions;
using System.Xml.Linq;
using KanbanBoardAPI.Models.KanbanRealm;
using KanbanBoardAPI.Models.KanbanDto;
using System.Linq;

namespace KanbanBoardAPI.Services.DbServices;

public class KanbanRealmService : IKanbanRealmService
{
    private readonly ILogger<KanbanRealmService> _logger;
    private readonly IWebAppConfig _webAppConfig;

    private readonly ulong _schemaVersion = 1;

    public KanbanRealmService(IWebAppConfig webAppConfig, ILogger<KanbanRealmService> logger)
    {
        _webAppConfig = webAppConfig;
        _logger = logger;
    }

    private Realm GetRealm()
    {
        try
        {
            RealmConfiguration kanbanDbConfig =
                new RealmConfiguration(_webAppConfig.KanbanRealmPath) { SchemaVersion = _schemaVersion };
            return Realm.GetInstance(kanbanDbConfig);
        }
        catch (RealmFileAccessErrorException ex)
        {
            _logger.LogCritical("Unable to find Realm, recreating...", ex.StackTrace);
            Directory.CreateDirectory(_webAppConfig.BasePath);

            RealmConfiguration kanbanDbConfig =
                new RealmConfiguration(_webAppConfig.KanbanRealmPath) { SchemaVersion = _schemaVersion };
            return Realm.GetInstance(kanbanDbConfig);
        }
    }



    public KanbanBoardDto CreateNewKanbanBoard(Guid userId, string name)
    {
        Realm realm = GetRealm();

        KanbanBoard board = new KanbanBoard() { User_Id = userId, Name = name, Id = Guid.NewGuid() };

        try
        {
            realm.Write(() =>
            {
                realm.Add(board);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Error writing to Realm: " + ex.StackTrace);
            throw new Exception("Unable to write to DB...");
        }

        return new KanbanBoardDto(board);
    }

    public KanbanBoardDto GetKanbanBoard(Guid userId)
    {
        KanbanBoard _board = null;
        Realm realm = GetRealm();
        _board = realm.All<KanbanBoard>().Where(x => x.User_Id == userId).FirstOrDefault();
        if (_board == null)
        {
            return CreateNewKanbanBoard(userId, "New Kanban Board");
        }

        KanbanBoardDto board = new KanbanBoardDto(_board);

        board.Columns = GetKanbanBoardColumns(userId, _board.Id).ToList();

        return board;
    }

    public IEnumerable<KanbanBoardDto> GetKanbanBoards(Guid userId)
    {
        Realm realm = GetRealm();
        List<KanbanBoardDto> _boards = new List<KanbanBoardDto>();
        foreach (KanbanBoard _board in realm.All<KanbanBoard>().Where(x => x.User_Id == userId))
        {
            KanbanBoardDto board = new KanbanBoardDto(_board);

            board.Columns = GetKanbanBoardColumns(userId, _board.Id).ToList();

            _boards.Add(board);
        }

        return _boards;
    }

    //TODO: Add deletions in for columns, cards, rows associated with board. 
    public bool DeleteKanbanBoard(Guid userId, Guid id)
    {
        Realm realm = GetRealm();

        try
        {
            realm.Write(() =>
            {
                KanbanBoard board = realm.All<KanbanBoard>().Where(x => x.User_Id == userId && x.Id == id).FirstOrDefault();
                if (board != null)
                {
                    realm.Remove(board);
                }
            });
        }
        catch (Exception e)
        {
            _logger.LogError("Could not delete board with ID: {0}" + e.StackTrace, id);
            return false;
        }
        return true;
    }



    public KanbanBoardColumnDto CreateNewKanbanColumn(Guid userId, Guid id, Guid KanbanBoard_Id, string name)
    {
        Realm realm = GetRealm();

        KanbanBoardColumn column = new KanbanBoardColumn()
        {
            Id = id,
            KanbanBoard_Id = KanbanBoard_Id,
            Name = name,
            User_Id = userId
        };

        try
        {
            realm.Write(() =>
            {
                realm.Add(column);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Error writing to Realm: " + ex.StackTrace);
            throw new Exception("Unable to write to DB...");
        }

        return new KanbanBoardColumnDto(column);
    }

    //TODO: Add deletions in for cards, rows associated with column. 
    public bool DeleteKanbanBoardColumn(Guid userId, Guid id)
    {
        Realm realm = GetRealm();

        try
        {
            realm.Write(() =>
            {
                KanbanBoardColumn column = realm.All<KanbanBoardColumn>().Where(x => x.User_Id == userId && x.Id == id).FirstOrDefault();
                if (column != null)
                {
                    realm.Remove(column);
                }
            });
        }
        catch (Exception e)
        {
            _logger.LogError("Could not delete board with ID: {0}" + e.StackTrace, id);
            return false;
        }
        return true;
    }

    public KanbanBoardColumnDto GetKanbanBoardColumn(Guid userId, Guid id)
    {
        KanbanBoardColumn _column = null;
        Realm realm = GetRealm();
        _column = realm.All<KanbanBoardColumn>().Where(x => x.User_Id == userId
            && x.Id == id).FirstOrDefault();
        if (_column == null)
        {
            _logger.LogError(string.Format("Board with Id {0} does not exist", id.ToString()));
            throw new Exception(string.Format("Board with Id {0} does not exist", id.ToString()));
        }

        KanbanBoardColumnDto column = new KanbanBoardColumnDto(_column);

        column.Cards = GetKanbanCards(userId, id).ToList();

        return column;
    }

    public IEnumerable<KanbanBoardColumnDto> GetKanbanBoardColumns(Guid userId, Guid kanbanBoard_Id)
    {
        Realm realm = GetRealm();
        List<KanbanBoardColumnDto> columns = new List<KanbanBoardColumnDto>();
        foreach (KanbanBoardColumn _column in realm.All<KanbanBoardColumn>().Where(x => x.User_Id == userId
            && x.KanbanBoard_Id == kanbanBoard_Id))
        {
            KanbanBoardColumnDto column = new KanbanBoardColumnDto(_column);

            column.Cards = GetKanbanCards(userId, column.Id).ToList();

            columns.Add(column);
        }

        return columns;
    }

    public KanbanBoardColumnDto UpdateKanbanBoardColumn(Guid userId, Guid id, Guid KanbanBoard_Id, string name)
    {
        Realm realm = GetRealm();

        KanbanBoardColumn column = new KanbanBoardColumn()
        {
            Id = id,
            KanbanBoard_Id = KanbanBoard_Id,
            Name = name,
            User_Id = userId
        };

        try
        {
            realm.Write(() =>
            {
                realm.Add(column, true);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Error writing to Realm: " + ex.StackTrace);
            throw new Exception("Unable to write to DB...");
        }

        return new KanbanBoardColumnDto(column);
    }



    public KanbanCardDto CreateKanbanCard(Guid userId, Guid id, Guid kanbanBoardColumn_Id, string name, string description, int index)
    {
        Realm realm = GetRealm();

        KanbanCard card = new KanbanCard()
        {
            Id = id,
            User_Id = userId,
            Description = description,
            IndexId = index,
            KanbanBoardColumn_Id = kanbanBoardColumn_Id,
            Name = name
        };

        try
        {
            realm.Write(() =>
            {
                realm.Add(card);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Error writing to Realm: " + ex.StackTrace);
            throw new Exception("Unable to write to DB...");
        }

        return new KanbanCardDto(card);
    }

    public KanbanCardDto GetKanbanCard(Guid userId, Guid id)
    {
        KanbanCard _card = null;
        Realm realm = GetRealm();
        _card = realm.All<KanbanCard>().Where(x => x.User_Id == userId
            && x.Id == id).FirstOrDefault();
        if (_card == null)
        {
            _logger.LogError(string.Format("Board with Id {0} does not exist", id.ToString()));
            throw new Exception(string.Format("Board with Id {0} does not exist", id.ToString()));
        }

        KanbanCardDto card = new KanbanCardDto(_card);

        card.CardRows = GetKanbanCardRows(userId, id).ToList();

        return card;
    }

    public IEnumerable<KanbanCardDto> GetKanbanCards(Guid userId, Guid columnId)
    {
        Realm realm = GetRealm();
        List<KanbanCardDto> cards = new List<KanbanCardDto>();
        foreach (KanbanCard _card in realm.All<KanbanCard>().Where(x => x.User_Id == userId
            && x.KanbanBoardColumn_Id == columnId))
        {
            KanbanCardDto card = new KanbanCardDto(_card);
            card.CardRows = GetKanbanCardRows(userId, card.Id).ToList();
            cards.Add(card);
        }

        return cards.OrderBy(x => x.IndexId);
    }

    //TODO: Add deletions in for rows associated with card. 
    public bool DeleteKanbanCard(Guid userId, Guid id)
    {
        Realm realm = GetRealm();

        try
        {
            realm.Write(() =>
            {
                KanbanCard card = realm.All<KanbanCard>().
                    Where(x => x.User_Id == userId && x.Id == id).FirstOrDefault();
                if (card != null)
                {
                    realm.Remove(card);
                }
            });
        }
        catch (Exception e)
        {
            _logger.LogError("Could not delete card with ID: {0}" + e.StackTrace, id);
            return false;
        }
        return true;
    }

    public KanbanCardDto UpdateKanbanBoardCard(Guid userId, Guid id, Guid kanbanBoardColumn_Id, string name, string description, int index)
    {
        Realm realm = GetRealm();

        KanbanCard card = new KanbanCard()
        {
            KanbanBoardColumn_Id = kanbanBoardColumn_Id,
            Id = id,
            User_Id = userId,
            Description = description,
            IndexId = index,
            Name = name
        };

        try
        {
            realm.Write(() =>
            {
                realm.Add(card, true);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Error writing to Realm: " + ex.StackTrace);
            throw new Exception("Unable to write to DB...");
        }

        return new KanbanCardDto(card);
    }



    public KanbanCardRowDto CreateKanbanCardRow(Guid userId, Guid id, Guid kanbanCard_Id, string description, bool completed)
    {
        Realm realm = GetRealm();

        KanbanCardRow row = new KanbanCardRow()
        {
            KanbanCard_Id = kanbanCard_Id,
            Id = id,
            User_Id = userId,
            Description = description,
            Completed = completed
        };

        try
        {
            realm.Write(() =>
            {
                realm.Add(row);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Error writing to Realm: " + ex.StackTrace);
            throw new Exception("Unable to write to DB...");
        }

        return new KanbanCardRowDto(row);
    }

    public KanbanCardRowDto GetKanbanCardRow(Guid userId, Guid id)
    {
        KanbanCardRow row = null;
        Realm realm = GetRealm();
        row = realm.All<KanbanCardRow>().Where(x => x.User_Id == userId
            && x.Id == id).FirstOrDefault();
        if (row == null)
        {
            _logger.LogError(string.Format("Board with Id {0} does not exist", id.ToString()));
            throw new Exception(string.Format("Board with Id {0} does not exist", id.ToString()));
        }

        return new KanbanCardRowDto(row);
    }

    public IEnumerable<KanbanCardRowDto> GetKanbanCardRows(Guid userId, Guid cardId)
    {
        Realm realm = GetRealm();
        List<KanbanCardRowDto> rows = new List<KanbanCardRowDto>();
        foreach (KanbanCardRow row in realm.All<KanbanCardRow>().Where(x => x.User_Id == userId
            && x.KanbanCard_Id == cardId))
        {
            rows.Add(new KanbanCardRowDto(row));
        }

        return rows;
    }

    public bool DeleteKanbanCardRow(Guid userId, Guid id)
    {
        Realm realm = GetRealm();

        try
        {
            realm.Write(() =>
            {
                KanbanCardRow row = realm.All<KanbanCardRow>().
                    Where(x => x.User_Id == userId && x.Id == id).FirstOrDefault();
                if (row != null)
                {
                    realm.Remove(row);
                }
            });
        }
        catch (Exception e)
        {
            _logger.LogError("Could not delete row with ID: {0}" + e.StackTrace, id);
            return false;
        }
        return true;
    }

    public KanbanCardRowDto UpdateKanbanBoardCardRow(Guid userId, Guid id, Guid kanbanCard_Id, string description, bool completed)
    {
        Realm realm = GetRealm();

        KanbanCardRow row = new KanbanCardRow()
        {
            KanbanCard_Id = kanbanCard_Id,
            Id = id,
            User_Id = userId,
            Description = description,
            Completed = completed
        };

        try
        {
            realm.Write(() =>
            {
                realm.Add(row, true);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Error writing to Realm: " + ex.StackTrace);
            throw new Exception("Unable to write to DB...");
        }

        return new KanbanCardRowDto(row);
    }
}