import { createContext, useContext, useState, ReactNode } from "react";
import {useEffect} from "react";
import {GetRootFolderPath} from "@/utilities/api-manager.ts";
import {LibraryFolderPathChunk} from "@/types/folders.ts";

type GlobalContextContainer = {
    rootFolderPath: string;
    currentFolderPath: LibraryFolderPathChunk[];
    setCurrentFolderPathFromApp: (path: LibraryFolderPathChunk[]) => void;

}

const GlobalContext = createContext<GlobalContextContainer | null>(null);

export function GlobalContextProvider({children}: {children: ReactNode}) {
    const [rootFolderPath, setRootFolderPath] = useState<string>("");
    const [currentFolderPath, setCurrentFolderPath] = useState<LibraryFolderPathChunk[]>([]);
    const [isInitialized, setIsInitialized] = useState(false);


    useEffect(() => {
        if (!isInitialized && !rootFolderPath) {
            async function fetchRootFolderPath() {
                try {
                    const response = await GetRootFolderPath();
                    setRootFolderPath(response.data);
                    setIsInitialized(true);
                } catch (error) {
                    console.error("Error fetching root folder path:", error);
                }
            }

            fetchRootFolderPath();
        }
    }, [isInitialized, rootFolderPath]);

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