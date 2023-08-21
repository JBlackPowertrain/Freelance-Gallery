import React, { useEffect } from 'react';
import titles from '../../data/titles.json'

export function usePageTitle() {  
    useEffect(() => {
        var title = titles.titles[Math.floor((Math.random() * (titles.titles.length - 1)))];
        document.title = title;
    }, []);
}