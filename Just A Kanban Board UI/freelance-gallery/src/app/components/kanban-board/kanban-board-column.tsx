"use client";

import React, { useState, useRef } from 'react'; 
import Image from 'next/image'

import { Droppable } from 'react-beautiful-dnd';

import { CreateKanbanCard, GetKanbanCards } from '../../api-calls/kanban-api-calls'
import apiPaths from "../../api-calls/api-paths.json"

import KanbanBoardCard from "./kanban-card-component"
import KanbanBoardCardProps from "./kanban-models/props/kanban-board-card-props"
import KanbanBoardColumnProps from "./kanban-models/props/kanban-board-column-props"

const KanbanBoardColumn = (props:KanbanBoardColumnProps) => {
    const [trackedCardsList, updateCards] = useState<KanbanBoardCardProps[]>([])
    const cardsList = useRef<KanbanBoardCardProps[]>([])
    const { data, error } = GetKanbanCards(apiPaths["dev-user-id"], props.id); 
    const dataRef = useRef<any>(null);

    function addNewCard() {
        var _props:KanbanBoardCardProps = {
            id: crypto.randomUUID(),
            indexId: cardsList.current.length,
            KanbanBoardColumn_Id: props.id,
            name: "New Card", 
            description: "Click here to start editing!",
            cardRows: [], 
            cardUpdated: handleCardUpdate,
            cardDeleted: cardDeleted
        };

        var tempList = [...cardsList.current];
        tempList.push(_props); 

        cardsList.current.push(_props); 
        updateCards([
            ...cardsList.current
        ]);

        let modifiedProps:KanbanBoardColumnProps = {
            id: props.id, 
            cards: tempList, 
            name: props.name, 
            cardsModified: props.cardsModified,
            columnDeletedCallback: props.columnDeletedCallback,
            columnUpdatedCallback: props.columnUpdatedCallback
        }
        props.cardsModified(modifiedProps);

        CreateKanbanCard(apiPaths['dev-user-id'], _props)
    } 

    function handleCardUpdate(cardProps: KanbanBoardCardProps) {
        console.log(cardsList.current); 
        cardsList.current.forEach(function(value:KanbanBoardCardProps, index:number){
            //Change the row info 
            if(value.id == cardProps.id){
                cardsList.current[index].cardRows = value.cardRows
            }
        }, cardsList);

        let modifiedProps:KanbanBoardColumnProps = {
            id: props.id, 
            cards: cardsList.current, 
            name: props.name, 
            cardsModified: props.cardsModified,
            columnDeletedCallback: props.columnDeletedCallback,
            columnUpdatedCallback: props.columnUpdatedCallback
        }

        props.cardsModified(modifiedProps);
    }

    function cardDeleted(cardProps: KanbanBoardCardProps){

    }

    if(dataRef.current == null && data != null && props.cards.length < 1){
        dataRef.current = data; 
        for(var x = 0; x < dataRef.current.length; x++){
            var _props:KanbanBoardCardProps = {
                id: dataRef.current[x].id,
                indexId: dataRef.current[x].indexId,
                KanbanBoardColumn_Id: dataRef.current[x].kanbanBoardColumn_Id,
                name: dataRef.current[x].name, 
                description: dataRef.current[x].description,
                cardRows: [], 
                cardUpdated: handleCardUpdate,
                cardDeleted: cardDeleted
            };
            cardsList.current.push(_props);
        }
        let modifiedProps:KanbanBoardColumnProps = {
            id: props.id, 
            cards: cardsList.current, 
            name: props.name, 
            cardsModified: props.cardsModified,
            columnDeletedCallback: props.columnDeletedCallback,
            columnUpdatedCallback: props.columnUpdatedCallback
        }
        props.cardsModified(modifiedProps);
    }
    else{
        console.log("Taking props now...");
        cardsList.current = props.cards;
    }
     

    return (
        <div className='border border-2 rounded kanaban-column'>
            <div className="kanaban-column-header">{props.name} 
                <span onClick={() =>{addNewCard();}}>
                    <Image alt="Add New Card" 
                    width={20} 
                    height={20} 
                    src={'/plus-solid.svg'}/>
            </span></div>
            <hr className="kanaban-column-header"/>
            <Droppable key={props.id} droppableId={props.id}>
                {(provided, snapshot) => (
                    <div className='droppable-minimum-height' ref={provided.innerRef} {...provided.droppableProps}>
                    {cardsList.current.map((item, index: number) =>(cardsList.current.length,
                        <KanbanBoardCard 
                        cardUpdated={handleCardUpdate}
                        cardDeleted={cardDeleted}
                        description={item.description}
                        name={item.name}
                        cardRows={[]} 
                        KanbanBoardColumn_Id={props.id} 
                        indexId={item.indexId} 
                        id={item.id} 
                        key={item.id} />
                    ))}
                    {provided.placeholder}
                    </div>
                )}
            </Droppable>
        </div>
    );
}

export default KanbanBoardColumn;