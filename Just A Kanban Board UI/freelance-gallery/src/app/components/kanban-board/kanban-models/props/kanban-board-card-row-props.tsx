interface KanbanBoardCardRowProps{
    id: string;
    kanbanCard_Id: string;
    completed: boolean;
    description: string;
    
    isEditing: boolean;

    rowUpdated: (props:KanbanBoardCardRowProps) => void; 
    rowDeleted:(props:KanbanBoardCardRowProps) => void; 
}

export default KanbanBoardCardRowProps;