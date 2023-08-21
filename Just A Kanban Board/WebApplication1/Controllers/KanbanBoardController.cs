using Microsoft.AspNetCore.Mvc;
using KanbanBoardAPI.Models.Kanban;
using KanbanBoardAPI.Services.DbServices;

namespace KanbanBoardAPI.Controllers
{
    [Route("KanbanBoard")]
    [ApiController]
    public class KanbanBoardController : ControllerBase
    {
        private readonly Guid devUser = Guid.Parse("42749177-7fd6-489e-87b2-e667b3383dd7"); 

        private readonly ILogger<KanbanBoardController> _logger;
        private readonly IKanbanRealmService _kanbanDbService; 

        public KanbanBoardController(ILogger<KanbanBoardController> logger, IKanbanRealmService kanbanDbService)
        {
            this._kanbanDbService = kanbanDbService;
            _logger = logger;
        }

        [HttpGet("{userId}")]
        public IActionResult Get(Guid userId)
        {
            return StatusCode(StatusCodes.Status200OK, _kanbanDbService.GetKanbanBoard(userId)); 
        }


        [HttpPost("column/create/{userId}")]
        public IActionResult CreateColumn(KanbanBoardColumn column, Guid userId)
        {
            return StatusCode(StatusCodes.Status200OK,
                _kanbanDbService.CreateNewKanbanColumn(userId, column.Id, column.KanbanBoard_Id, column.Name));
        }

        [HttpPost("column/save/{userId}")]
        public IActionResult SaveColumn(KanbanBoardColumn column, Guid userId)
        {
            return StatusCode(StatusCodes.Status200OK,
                _kanbanDbService.UpdateKanbanBoardColumn(userId, column.Id,
                column.KanbanBoard_Id, column.Name));
        }

        [HttpDelete("column/delete/{userId}/{columnId}")]
        public IActionResult DeleteColumn(Guid userId, Guid columnId)
        {
            return StatusCode(StatusCodes.Status200OK, 
                _kanbanDbService.DeleteKanbanBoardColumn(userId, columnId)); 
        }
    }
}
