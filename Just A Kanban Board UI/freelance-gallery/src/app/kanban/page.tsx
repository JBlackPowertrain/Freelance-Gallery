"use client";

import React from 'react'; 

import KanbanBoard  from "../components/kanban-board/kanban-board";

import { usePageTitle } from "../components/common/common-effects"

export default function Kanban() {
    usePageTitle(); 
    return (
        <KanbanBoard />
    );
}