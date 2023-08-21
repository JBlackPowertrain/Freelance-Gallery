"use client";

//For debug only...
import apiPaths from "../../api-calls/api-paths.json"

import React, { useState, useRef } from 'react'; 

import { DragDropContext } from 'react-beautiful-dnd';

import { GetKanbanBoard, CreateKanbanColumn, IndexKanbanCards } from '../../api-calls/kanban-api-calls'

import KanbanBoardColumn  from "./kanban-board-column";
import KanbanBoardCardProps from "./kanban-models/props/kanban-board-card-props"
import KanbanBoardColumnProps from "./kanban-models/props/kanban-board-column-props"

export default function KanbanBoard(props:any){
    const [columns, setColumns] = useState<KanbanBoardColumnProps[]>([]);
    
    //Do not want to update the board each time a column has been edited. Only column should redraw. 
    const trackedColumns = useRef<KanbanBoardColumnProps[]>([]);

    //Do not want to update state when cards are added; Don't want to redraw the whole board. 
    const cards = useRef<KanbanBoardCardProps[]>([]);
    const { data, error } = GetKanbanBoard(apiPaths["dev-user-id"]); 
    const dataRef = useRef<any>(); 

    const buttonEnabled = useRef<boolean>(true); 

    const [_updateState, updateState] = useState(0);

    //called when a new column is added
    async function addNewColumn(){
        var _id = "" + columns.length;
        var columnProps: KanbanBoardColumnProps = {
            id: crypto.randomUUID(),
            name: "New Kanban Column",
            cards: [],
            cardsModified: cardsModified,
            columnDeletedCallback: columnDeleted,
            columnUpdatedCallback: columnModified
        };

        trackedColumns.current.push(columnProps);

        setColumns([
            ...columns,
            columnProps                
        ])

        CreateKanbanColumn(apiPaths["dev-user-id"], dataRef.current.id, columnProps);
    }

    //Moves a card from within a column, re-ordering them and re-indexing them. 
    //redraws the effected column...needs optimized. Okay for smaller projects but larger boards may lag due to poor algorithm
    function moveCardInColumn(result:any){

        let cardsForColumn:KanbanBoardCardProps[] = []; 
        let newIndex:number = result.destination.index; 
        let droppableId:string = result.destination.droppableId; 
        let draggableId:string = result.draggableId; 

        var newColumnsList:KanbanBoardColumnProps[] = [...trackedColumns.current]; 

        trackedColumns.current = []; 
        newColumnsList.forEach(function(column:KanbanBoardColumnProps, columnIndex:number){
            if(column.id == droppableId){
                cardsForColumn = [...column.cards]; 
                let card:KanbanBoardCardProps[] = []; 
                cardsForColumn.forEach(function(value:KanbanBoardCardProps, index:number) {
                    if(value.id == draggableId) { 
                        card = cardsForColumn.splice(index, 1);
                    } 
                });
                
                if(card.length < 1) { return; }

                cardsForColumn.splice(newIndex, 0, card[0]);

                for(let x = 0; x < cardsForColumn.length; x++) {
                    cardsForColumn[x].indexId = x; 
                }

                column.cards = cardsForColumn; 
                IndexKanbanCards(apiPaths["dev-user-id"], column.id, cardsForColumn);
            }
            trackedColumns.current.push(column); 
        });

        setColumns([
            ...trackedColumns.current
        ]);

    }

    //Re-indexes the cards in affected columns; Redraws the board. 
    function moveCardToNewColumn(result:any){
        console.log(result);
        console.log("Move to column");

    }

    //Called when a column is modified; Pushed from kanban-column-component
    function columnModified(props:KanbanBoardColumnProps) {

    }

    //Called when a column is deleted; Pushed from kanban-column-component
    function columnDeleted(props:KanbanBoardColumnProps) {

    }

    //Called from Column when cards are modified; Pushed up from kanban-card-component
    function cardsModified(props_:KanbanBoardColumnProps) {
        console.log("Props");
        console.log(props_);
        let copyArray:KanbanBoardCardProps[] = [...cards.current];
        let removeindices:number[] = []; 
        cards.current.forEach(function(value:KanbanBoardCardProps, index:number){
            if(cards.current[index].KanbanBoardColumn_Id === props_.id){
                removeindices.unshift(index); 
            }
        });

        removeindices.forEach(function(index:number){
            copyArray.splice(index, 1);
        });

        cards.current = [...copyArray];
        cards.current = cards.current.concat(props_.cards) 

        trackedColumns.current.forEach(function(column:KanbanBoardColumnProps, columnIndex:number) {
            if(props_.id == column.id) {
                trackedColumns.current[columnIndex] = props_;      
            }
        });
    }

    //Called when a drag event happens on a card; Pushed from kanban-board-component
    const onDragEnd = (result:any) => {
        if(result.destination == null){ return; }

        let droppableId:string = result.destination.droppableId; 
        let draggableId:string = result.draggableId; 

        for(let card of cards.current) {
            if(card.id === "" + draggableId){
                if(card.KanbanBoardColumn_Id === droppableId){
                    moveCardInColumn(result);
                }
                else{
                    console.log(card);
                    console.log(draggableId);
                    console.log(droppableId);
                    moveCardToNewColumn(result);
                }            
                break;
            }
        }

        return;
    };    
    
    //if(error){
    //    alert("Could not load data. Reload page; Error handling not properly implemented yet...");
    //}

    if(dataRef.current == null && data != null){
        dataRef.current = data;
        buttonEnabled.current = false; 
        let columnsFromDb:KanbanBoardColumnProps[] = []; 

        for(var x = 0; x < dataRef.current.columns.length; x++){
            var columnProps: KanbanBoardColumnProps = {
                id: dataRef.current.columns[x].id,
                name: dataRef.current.columns[x].name,
                cards: [],
                cardsModified: cardsModified,
                columnDeletedCallback: columnDeleted,
                columnUpdatedCallback: columnModified
            };
            trackedColumns.current.push(columnProps); 
            columnsFromDb.push(columnProps); 
        }

        setColumns([...columnsFromDb]);
    }
    
    return(            
        <DragDropContext onDragEnd={(result) => onDragEnd(result)}>
            <div className="kanban-board">
                {Object.entries(columns).map(([columnId, column]) => {
                    return (
                        <KanbanBoardColumn key={columnId} {...column} />
                    );
                })}
                <button disabled={buttonEnabled.current} className='addNewButton' onClick={addNewColumn}>New Column</button>
            </div>
        </DragDropContext>
    );
}