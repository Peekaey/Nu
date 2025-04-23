import {ImageCard} from "@/components/image-card.tsx";
import {GetRootLibraryContents} from "@/utilities/api-manager.ts";
import { useState, useEffect } from "react";
import { LibraryFolderFilesResponseDTO
} from "@/types/folders.ts";
import {LibraryAlbumsResponseDTO} from "@/types/library.ts";
import { AlbumCard } from "@/components/album-card";
import {useNavigate} from "react-router";
import {LoadingAlert} from "@/components/loading-alert.tsx";
import {NoContentsAlert} from "@/components/no-contents-alert.tsx";

export function LibraryHomePage() {
    const [files, setFiles] = useState<LibraryFolderFilesResponseDTO[]>([]);
    const [folders, setFolders] = useState<LibraryAlbumsResponseDTO[]>([]);
    const [loading, setLoading] = useState(true); // Add loading state

    const navigate = useNavigate();



    useEffect(() => {
        // Fetch Initial Folders
        async function fetchFolderContents() {
            try {
                const response = await GetRootLibraryContents();
                console.log("Full Response:", response);
                setFolders(response.data.folders);
                setFiles(response.data.files);
            } catch (error) {
                console.error("Error fetching folders:", error);
            } finally {
                setLoading(false);
            }
        }

        fetchFolderContents();
    }, []);

    const handleAlbumClick = (id: string) => {
        navigate(`/library/folder/${id}`);
    };


    return (
        <div id="library-page" className="w-full">
            {loading && <LoadingAlert />}
            {files.length === 0 && folders.length === 0 && !loading && <NoContentsAlert/>}
            <div id="library-body-content" className="grid grid-cols-2 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-4 2xl:grid-cols-6 gap-4 pt-4 pb-4 pr-4 pl-9">
                {folders.map((folder) => (
                    <AlbumCard
                        key={folder.id}
                        id={folder.id}
                        title={folder.albumName}
                        folderPath={folder.albumPath}
                        onClick={() => {
                            handleAlbumClick(folder.id);
                        }}
                    />
                ))}
                {files.map((file, index) => (
                    <ImageCard
                        key={index}
                        src={file.imageData}
                        alt={"Image"}
                        title={file.fileName || "Untitled"}
                        description={file.filePath || "No description available"}
                    />
                ))}
            </div>
        </div>
    );
}