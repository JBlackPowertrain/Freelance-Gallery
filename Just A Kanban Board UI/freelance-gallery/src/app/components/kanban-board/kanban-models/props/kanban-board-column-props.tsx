import KanbanBoardCardProps from "./kanban-board-card-props"

interface KanbanBoardColumnProps{
    id: string;
    name: string;

    cards: KanbanBoardCardProps[];

    cardsModified: (props:KanbanBoardColumnProps) => void; 

    columnUpdatedCallback: (props:KanbanBoardColumnProps) => void; 
    columnDeletedCallback: (props: KanbanBoardColumnProps) => void; 
    //cardReorderCallback: (columnId:any) => void; 
}

export default KanbanBoardColumnProps;