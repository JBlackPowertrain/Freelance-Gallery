import { useState, useEffect, useRef } from 'react';
import apiPaths from "./api-paths.json"

import KanbanBoardColumn  from "../components/kanban-board/kanban-models/props/kanban-board-column-props"
import KanbanBoardCard from "../components/kanban-board/kanban-models/props/kanban-board-card-props"
import KanbanBoardCardRow from "../components/kanban-board/kanban-models/props/kanban-board-card-row-props"


  export function GetKanbanBoard(userId:string) {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
      async function fetchData() {
        try {
          const response = await fetch(apiPaths.kanbanGet + "/" + userId);
          const data = await response.json();
          console.log(data);
          setData(data);
        } catch (error:any) {
          setError(error);
        } finally {
          setLoading(false);
        }
      }
  
      fetchData();
    }, [userId]);
  
    return { data, loading, error };
  }

  export function GetKanbanCards(userId:string, columnId:string) {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
      async function fetchData() {
        try {
          const response = await fetch(apiPaths.kanbanCardsGet + "/" + userId + "/" + columnId);
          const data = await response.json();
          console.log(data);
          setData(data);
        } catch (error:any) {
          setError(error);
        } finally {
          setLoading(false);
        }
      }
  
      fetchData();
    }, [userId, columnId]);
  
    return { data, loading, error };
  }

  export function GetKanbanCardRows(userId:string, cardId:string) {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
      async function fetchData() {
        try {
          const response = await fetch(apiPaths.kanbanCardRowsGet + "/" + userId + "/" + cardId);
          const data = await response.json();
          console.log(data);
          setData(data);
        } catch (error:any) {
          setError(error);
        } finally {
          setLoading(false);
        }
      }
  
      fetchData();
    }, [userId, cardId]);
  
    return { data, loading, error };
  }

  export function CreateKanbanColumn(userId:string, boardId:string, columnProps:KanbanBoardColumn){
    const requestOptions = {
      method: "POST", 
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ id:columnProps.id, name:columnProps.name, KanbanBoard_Id:boardId })
    }
    fetch(apiPaths.kanbanColumnCreate + "/" + userId, requestOptions)
    .then(async response => {
        const data = await response.body;

        // check for error response
        if (!response.ok) {
            // get error message from body or default to response status
            const error = (data) || response.status;
            return Promise.reject(error);
        }
    })
    .catch(error => {
        //alert({ errorMessage: error.toString() });
        console.error('There was an error!', error);
    });
  }

  export function SaveKanbanColumn(userId:string, boardId:string, columnProps:KanbanBoardColumn){
    const requestOptions = {
      method: "POST", 
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ id:columnProps.id, name:columnProps.name, KanbanBoard_Id:boardId })
    }
    fetch(apiPaths.kanbanColumnSave + "/" + userId, requestOptions)
    .then(async response => {
        const data = await response.body;

        // check for error response
        if (!response.ok) {
            // get error message from body or default to response status
            const error = (data) || response.status;
            return Promise.reject(error);
        }
    })
    .catch(error => {
        //alert({ errorMessage: error.toString() });
        console.error('There was an error!', error);
    });
  }

  export function DeleteKanbanColumn(userId:string, columnProps:KanbanBoardColumn){
    const requestOptions = {
      method: "DELETE"
    }
    fetch(apiPaths.kanbanColumnDelete + "/" + userId + "/" + columnProps.id, requestOptions)
    .then(async response => {
        const data = await response.body;

        // check for error response
        if (!response.ok) {
            // get error message from body or default to response status
            const error = (data) || response.status;
            return Promise.reject(error);
        }
    })
    .catch(error => {
        //alert({ errorMessage: error.toString() });
        console.error('There was an error!', error);
    });
  }



  export function CreateKanbanCard(userId:string, cardProps:KanbanBoardCard) {
    const requestOptions = {
      method: "POST", 
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(cardProps)
    }
    console.log(requestOptions);
    fetch(apiPaths.kanbanCardCreate + "/" + userId, requestOptions)
    .then(async response => {
        const data = await response.body;

        // check for error response
        if (!response.ok) {
            console.log(response.body);
            // get error message from body or default to response status
            const error = (data) || response.status;
            return Promise.reject(error);
        }
    })
    .catch(error => {
        //alert({ errorMessage: error.toString() });
        console.error('There was an error!', error);
    });
  }

  export function SaveKanbanCard(userId:string, cardProps:KanbanBoardCard) {
    const requestOptions = {
      method: "POST", 
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(cardProps)
    }
    fetch(apiPaths.kanbanCardSave + "/" + userId, requestOptions)
    .then(async response => {
        const data = await response.body;

        // check for error response
        if (!response.ok) {
            // get error message from body or default to response status
            const error = (data) || response.status;
            return Promise.reject(error);
        }
    })
    .catch(error => {
        //alert({ errorMessage: error.toString() });
        console.error('There was an error!', error);
    });
  }

  export function DeleteKanbanCard(userId:string, cardProps:KanbanBoardCard) {
    const requestOptions = {
      method: "DELETE"
    }
    fetch(apiPaths.kanbanCardDelete + "/" + userId, requestOptions)
    .then(async response => {
        const data = await response.body;

        // check for error response
        if (!response.ok) {
            // get error message from body or default to response status
            const error = (data) || response.status;
            return Promise.reject(error);
        }
    })
    .catch(error => {
        //alert({ errorMessage: error.toString() });
        console.error('There was an error!', error);
    });
  }

  export function IndexKanbanCards(userId:string, columnId:string, cardProps:KanbanBoardCard[]) {
    const requestOptions = {
      method: "POST", 
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(cardProps)
    }
    console.log("PROPS");
    console.log(cardProps);
    console.log(JSON.stringify(cardProps));
    fetch(apiPaths.kanbanCardIndex + "/" + userId + "/" + columnId, requestOptions)
    .then(async response => {
        const data = await response.body;

        // check for error response
        if (!response.ok) {
            // get error message from body or default to response status
            const error = (data) || response.status;
            return Promise.reject(error);
        }
    })
    .catch(error => {
        //alert({ errorMessage: error.toString() });
        console.error('There was an error!', error);
    });
  }



  export function CreateKanbanCardRow(userId:string, cardId:string, cardRowProps:KanbanBoardCardRow) {
    const requestOptions = {
      method: "POST", 
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(cardRowProps)
    }
    fetch(apiPaths.kanbanCardRowCreate + "/" + userId, requestOptions)
    .then(async response => {
        const data = await response.body;

        // check for error response
        if (!response.ok) {
            // get error message from body or default to response status
            const error = (data) || response.status;
            return Promise.reject(error);
        }
    })
    .catch(error => {
        //alert({ errorMessage: error.toString() });
        console.error('There was an error!', error);
    });
  }

  export function SaveKanbanCardRow(userId:string, cardId:string, cardRowProps:KanbanBoardCardRow) {
    const requestOptions = {
      method: "POST", 
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(cardRowProps)
    }
    fetch(apiPaths.kanbanCardRowSave + "/" + userId, requestOptions)
    .then(async response => {
        const data = await response.body;

        // check for error response
        if (!response.ok) {
            // get error message from body or default to response status
            const error = (data) || response.status;
            return Promise.reject(error);
        }
    })
    .catch(error => {
        //alert({ errorMessage: error.toString() });
        console.error('There was an error!', error);
    });
  }

  export function DeleteKanbanCardRow(userId:string, cardRowProps:KanbanBoardCardRow) {
    const requestOptions = {
      method: "DELETE"
    }
    fetch(apiPaths.kanbanCardRowDelete + "/" + userId + "/" + cardRowProps.id, requestOptions)
    .then(async response => {
        const data = await response.body;

        // check for error response
        if (!response.ok) {
            // get error message from body or default to response status
            const error = (data) || response.status;
            return Promise.reject(error);
        }
    })
    .catch(error => {
        //alert({ errorMessage: error.toString() });
        console.error('There was an error!', error);
    });
  }