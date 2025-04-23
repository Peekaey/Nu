import { createContext, useContext, useState, ReactNode } from "react";
import {useEffect} from "react";
import {GetRootFolderPath} from "@/utilities/api-manager.ts";

type GlobalContextContainer = {
    rootFolderPath: string;
    currentFolderPath: string;
    setCurrentFolderPathFromApp: (path: string) => void;

}

const GlobalContext = createContext<GlobalContextContainer | null>(null);

export function GlobalContextProvider({children}: {children: ReactNode}) {
    const [rootFolderPath, setRootFolderPath] = useState<string>("");
    const [currentFolderPath, setCurrentFolderPath] = useState<string>("");

    // TODO Look at making this a one time call to avoid redundant calls
    useEffect(() => {
        async function fetchRootFolderPath() {
            try {
                const response = await GetRootFolderPath();
                setRootFolderPath(response.data);
            } catch (error) {
                console.error("Error fetching root folder path:", error);
            }
        }

        fetchRootFolderPath();
    }, []);

    return (
        <GlobalContext.Provider value={{ rootFolderPath, currentFolderPath, setCurrentFolderPathFromApp: setCurrentFolderPath }}>            {children}
        </GlobalContext.Provider>
    );
}

export function useGlobalContext() {
    const context = useContext(GlobalContext);
    if (!context) {
        throw new Error("useGlobalContext must be used within a GlobalContextProvider");
    }
    return context;
}