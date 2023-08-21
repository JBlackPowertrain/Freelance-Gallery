"use client";
import React, { useState, useRef } from 'react'; 

import { SaveKanbanCardRow } from '../../api-calls/kanban-api-calls'
import apiPaths from "../../api-calls/api-paths.json"

import KanbanBoardCardRowProps from "./kanban-models/props/kanban-board-card-row-props"

//Component for kanban board; Contains checkbox for tasks
export default function KanbanBoardCardRow(props:KanbanBoardCardRowProps){

    var editing:boolean = true;

    const [id, setId] = useState('');
    const description = useRef('');
    const [isChecked, setChecked] = useState(props.completed);
    const [isEditing, setEditing] = useState(props.isEditing);
    console.log(props);
    function finishEditing(desc:string){
        description.current = (desc);
        setEditing(!editing);

        editing = !editing;

        let updatedProps:KanbanBoardCardRowProps = {
            id: props.id,
            description: description.current,
            isEditing: isEditing,
            completed: isChecked,
            kanbanCard_Id: props.kanbanCard_Id,
            rowDeleted: props.rowDeleted,
            rowUpdated: props.rowUpdated
        };
        props.rowUpdated(updatedProps); 
        SaveKanbanCardRow(apiPaths["dev-user-id"], props.kanbanCard_Id, updatedProps);
    }

    function handleChecked(){
        let _checked:boolean = !isChecked; 
        setChecked(!isChecked);
        let updatedProps:KanbanBoardCardRowProps = {
            id: props.id,
            description: description.current,
            isEditing: isEditing,
            completed: _checked,
            kanbanCard_Id: props.kanbanCard_Id,
            rowDeleted: props.rowDeleted,
            rowUpdated: props.rowUpdated
        };
        props.rowUpdated(updatedProps); 
        SaveKanbanCardRow(apiPaths["dev-user-id"], props.kanbanCard_Id, updatedProps);
    }

    function isEmptyOrSpaces(str:string){
        return str === null || str.match(/^ *$/) !== null;
    }

    if(description.current.length == 0){
        description.current = props.description; 
    }

    if(!description.current?.toString().trim() && !editing){
        setEditing(true);
    }

    return (
        <div>
            <label className={isEditing ? "hide-kanaban-card-task" : "show-kanaban-card-task"}>
                <input type="checkbox" 
                    readOnly
                    checked={isChecked}
                    onClick={() => handleChecked()} />
                {description.current}
            </label>
            <input className={isEditing ? "show-kanaban-card-task" : "hide-kanaban-card-task"}
                type="text" 
                placeholder="Enter New Task"
                onBlur={evt => {
                    if(!isEmptyOrSpaces(evt.target.value)){
                        finishEditing(evt.target.value.trimStart().trimEnd());
                    }
                }}/>
        </div>  
    );
}; 