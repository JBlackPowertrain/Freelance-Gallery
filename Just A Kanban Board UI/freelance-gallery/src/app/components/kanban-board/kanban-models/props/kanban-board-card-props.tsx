import KanbanBoardCardRowProps from "./kanban-board-card-row-props"

interface KanbanBoardCardProps{
    id: string;
    KanbanBoardColumn_Id: string;
    indexId: number;

    description: string, 
    name: string,

    cardRows: KanbanBoardCardRowProps[];

    cardUpdated: (props:KanbanBoardCardProps) => void; 
    cardDeleted: (props:KanbanBoardCardProps) => void; 
}

export default KanbanBoardCardProps;