"use client";
import React, { useRef, useState } from 'react'; 
import Image from 'next/image'

import { Draggable } from 'react-beautiful-dnd';

import { SaveKanbanCard, CreateKanbanCardRow, GetKanbanCardRows } from '../../api-calls/kanban-api-calls'
import apiPaths from "../../api-calls/api-paths.json"

import KanbanCardRowComponent from "./kanban-card-row-component"
import KanbanBoardCardRowProps from "./kanban-models/props/kanban-board-card-row-props"
import KanbanBoardCardProps from "./kanban-models/props/kanban-board-card-props"

export default function KanbanBoardCard(props:KanbanBoardCardProps){
    const [rowListState, setRowListState] = useState<KanbanBoardCardRowProps[]>([]);
    const [isEditng, setEditing] = useState(false);
    const rowList = useRef<KanbanBoardCardRowProps[]>([]);

    const editingDesc = useRef<string>(props.description);
    const editingName = useRef<string>(props.name);

    const { data, error } = GetKanbanCardRows(apiPaths["dev-user-id"], props.id); 
    const dataRef = useRef<any>(null);


    //probably redraws the entire card because the ItemList state is changed each time. 
    function rowsUpdated(rowProps:KanbanBoardCardRowProps) {
        let newRowNeeded:boolean = true; 

        let iterationList:KanbanBoardCardRowProps[] = [...rowList.current]; 
        iterationList.forEach(function(value:KanbanBoardCardRowProps, index:number){
            //Change the row info 
            if(value.id == rowProps.id){
                console.log(rowProps); 
                rowList.current[index] = rowProps; 
            }else{

                if(rowList.current[index].description.trim() == ""){
                    console.log("---------------");
                    console.log(rowList.current);
                    console.log("Row NOT needed");
                    newRowNeeded = false; 
                } 
            }
        });
        

        if(newRowNeeded) { addRow(); }

        let newProps:KanbanBoardCardProps = {
            id: props.id,
            KanbanBoardColumn_Id: props.KanbanBoardColumn_Id,
            description: props.description, 
            name: props.name, 
            indexId: props.indexId,

            cardRows: rowList.current, 

            cardUpdated: props.cardUpdated,
            cardDeleted: props.cardUpdated 
        }
        //Push to column
        props.cardUpdated(newProps); 
    }

    function rowsDeleted(rowProps:KanbanBoardCardRowProps){
        rowList.current.forEach(function(value:KanbanBoardCardRowProps, index:number){
            //check to see if the row needs added 
            if(value.id == rowProps.id){
                rowList.current.splice(index, 1);  
                setRowListState([...rowList.current]); 
            }
        }, rowList.current);

        //Push to column
        props.cardUpdated(props);
    }

    function addRow(){
        var rowProps: KanbanBoardCardRowProps = {
            description: "",
            completed: false,
            isEditing: true,
            id: crypto.randomUUID(),
            kanbanCard_Id: props.id,
    
            rowUpdated: rowsUpdated,
            rowDeleted: rowsDeleted
        };
        
        rowList.current.push(rowProps); 
        setRowListState([...rowList.current]);
        CreateKanbanCardRow(apiPaths["dev-user-id"], props.id, rowProps);  
    }

    function doEditing() {
        var rowProps: KanbanBoardCardProps = {
            description: editingDesc.current,
            name: editingName.current,
            indexId: props.indexId,
            id: props.id,
            KanbanBoardColumn_Id: props.KanbanBoardColumn_Id,
            cardUpdated: props.cardUpdated,
            cardDeleted: props.cardDeleted,
            cardRows: props.cardRows
        };
        props.cardUpdated(rowProps);
        setEditing(!isEditng);
        SaveKanbanCard(apiPaths["dev-user-id"], rowProps);
    }



    if(dataRef.current == null && data != null && props.cardRows.length < 1){
        dataRef.current = data; 
        for(var x = 0; x < dataRef.current.length; x++){
            var rowProps: KanbanBoardCardRowProps = {
                description: dataRef.current[x].description,
                completed: dataRef.current[x].completed,
                isEditing: (dataRef.current[x].description. length===0) ? true : false,
                id: dataRef.current[x].id,
                kanbanCard_Id: props.id,
        
                rowUpdated: rowsUpdated,
                rowDeleted: rowsDeleted
            };
            
            rowList.current.push(rowProps); 
        }
        console.log(rowList); 
        setRowListState([...rowList.current]);
        
        if(rowList.current.length < 1){
            addRow(); 
        }
    }


    return(
        //When editing is enabled and a user clicks on an empty row, the user can fill in the row and a new one should appear
        //If the user finishes editting the card and there are any empty rows, they are hidden and will not be sent to the server
        //Parent component pulls props which contains the KanbanBoardCardRow props. 
        <Draggable key={props.id} draggableId={props.id} index={props.indexId}>
        {(provided) => (
            <div
            ref={provided.innerRef}
            {...provided.draggableProps}
            {...provided.dragHandleProps}>
                <div className="rounded kanban-card">
                    <div className="kanaban-card-header">{editingName.current}
                        <span 
                            className='editButton'
                            onClick={() => { doEditing(); }}>
                                <Image alt="Add New Card" 
                                width={20} 
                                height={20} 
                                src={ isEditng ? '/check-solid.svg' : '/pen-to-square-solid.svg'}/>
                    </span>
                    </div>
                    <hr className="kanaban-column-header"/>
                    { isEditng ? <textarea onChange={evt => editingDesc.current = evt.target.value.trimStart().trimEnd()}>{editingDesc.current}</textarea> 
                        : <div className="kanaban-card-descriptions">{editingDesc.current}</div>}
                        {rowList.current.map(item =>(
                            rowList.current.length,
                            <KanbanCardRowComponent key={item.id} {...item}/>
                        ))}
                </div>
            </div>
        )}
        </Draggable>
    );
}; 


