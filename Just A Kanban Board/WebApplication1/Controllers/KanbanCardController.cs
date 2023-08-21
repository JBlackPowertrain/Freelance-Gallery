using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KanbanBoardAPI.Models.Kanban;
using KanbanBoardAPI.Services.DbServices;
using KanbanBoardAPI.Models.KanbanDto;

namespace KanbanBoardAPI.Controllers
{
    [Route("kanbanboard/KanbanCard")]
    [ApiController]
    public class KanbanCardController : ControllerBase
    {
        private readonly Guid devUser = Guid.Parse("42749177-7fd6-489e-87b2-e667b3383dd7");

        private readonly IKanbanRealmService _kanbanDbService;
        private readonly ILogger<KanbanCardController> _logger;

        public KanbanCardController(ILogger<KanbanCardController> logger, IKanbanRealmService kanbanDbService)
        {
            _logger = logger;
            _kanbanDbService = kanbanDbService;
        }

        [HttpGet("/{userId}/{cardId}")]
        public IActionResult GetCard(Guid userId, Guid cardId)
        {
            return StatusCode(StatusCodes.Status200OK, _kanbanDbService.GetKanbanCard(userId, cardId));
        }

        [HttpGet("column_cards/{userId}/{columnId}")]
        public IActionResult GetColumnCards(Guid userId, Guid columnId)
        {
            return StatusCode(StatusCodes.Status200OK, _kanbanDbService.GetKanbanCards(userId, columnId));
        }

        [HttpPost("update_card/{userId}")]
        public IActionResult UpdateCard(KanbanCard card, Guid userId)
        {
            return StatusCode(StatusCodes.Status200OK, 
                _kanbanDbService.UpdateKanbanBoardCard(userId, 
                card.Id, 
                card.KanbanBoardColumn_Id, 
                card.Name, 
                card.Description, 
                card.IndexId));
        }

        [HttpPost("create_card/{userId}")]
        public IActionResult CreateCard(KanbanCard card, Guid userId)
        {
            return StatusCode(StatusCodes.Status200OK,
                _kanbanDbService.CreateKanbanCard(userId,
                card.Id,
                card.KanbanBoardColumn_Id,
                card.Name,
                card.Description,
                card.IndexId));
        }

        [HttpPost("update_card_indices/{userId}/{columnId}")]
        public IActionResult UpdateCardIndices(IList<KanbanCard> cards, Guid columnId, Guid userId)
        {
            foreach(KanbanCard card in cards)
            {
                _kanbanDbService.UpdateKanbanBoardCard(userId, 
                    card.Id, 
                    card.KanbanBoardColumn_Id, 
                    card.Name, 
                    card.Description, 
                    card.IndexId);
            }

            return StatusCode(StatusCodes.Status200OK, _kanbanDbService.GetKanbanCards(userId, columnId));
        }

        [HttpDelete("delete_card/{userId}/{cardId}")]
        public IActionResult DeleteCard(Guid userId, Guid cardId)
        {
            return StatusCode(StatusCodes.Status200OK, _kanbanDbService.DeleteKanbanCard(userId, cardId)); 
        }



        [HttpGet("card_row/{userId}/{rowId}")]
        public IActionResult GetCardRow(Guid userId, Guid rowId)
        {
            return StatusCode(StatusCodes.Status200OK, _kanbanDbService.GetKanbanCardRow(userId, rowId));
        }

        [HttpGet("card_rows/{userId}/{cardId}")]
        public IActionResult GetCardRows(Guid userId, Guid cardId)
        {
            return StatusCode(StatusCodes.Status200OK, _kanbanDbService.GetKanbanCardRows(userId, cardId));
        }

        [HttpPost("card_rows/create/{userId}")]
        public IActionResult CreateCardRow(KanbanCardRow row, Guid userId)
        {
            return StatusCode(StatusCodes.Status200OK, 
                _kanbanDbService.CreateKanbanCardRow(userId, 
                row.Id, 
                row.KanbanCard_Id, 
                row.Description, 
                row.Completed));
        }

        [HttpPost("card_rows/update/{userId}")]
        public IActionResult UpdateCardRow(KanbanCardRow row, Guid userId)
        {
            return StatusCode(StatusCodes.Status200OK, 
                _kanbanDbService.UpdateKanbanBoardCardRow(userId, 
                row.Id, 
                row.KanbanCard_Id, 
                row.Description, 
                row.Completed));
        }

        [HttpDelete("card_rows/delete/{userId}/{rowId}")]
        public IActionResult DeleteCardRow(Guid userId, Guid rowId)
        {
            return StatusCode(StatusCodes.Status200OK, _kanbanDbService.DeleteKanbanCardRow(userId, rowId));
        }


    }
}
