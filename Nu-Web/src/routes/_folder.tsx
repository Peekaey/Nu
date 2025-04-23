
import { useParams } from "react-router-dom";
import {LibraryFolderFilesResponseDTO} from "@/types/folders.ts";
import {ImageCard} from "@/components/image-card.tsx";
import {useEffect, useState} from "react";
import { GetLibraryFolderContents} from "@/utilities/api-manager.ts";
import {AlbumCard} from "@/components/album-card.tsx";
import {LibraryAlbumsResponseDTO} from "@/types/library.ts";
import {useNavigate} from "react-router";
import {LoadingAlert} from "@/components/loading-alert.tsx";
import {NoContentsAlert} from "@/components/no-contents-alert.tsx";
import {FolderBreadcrumb} from "@/components/folder-breadcrumb.tsx";
import {useGlobalContext} from "@/components/global-context-container.tsx";


export function FolderView() {
    const { id } = useParams();
    const [files, setFiles] = useState<LibraryFolderFilesResponseDTO[]>([]);
    const [folders, setFolders] = useState<LibraryAlbumsResponseDTO[]>([]);
    const [loading, setLoading] = useState(true);
    const { setCurrentFolderPathFromApp } = useGlobalContext();
    const navigate = useNavigate();

    useEffect(() => {
        if (!id) {
            console.error("No folder id provided in the URL.");
            return;
        }

        const fetchFolderContents = async () => {
            setLoading(true);
            setFiles([]);
            setFolders([]);
            try {
                const response = await GetLibraryFolderContents(id);
                console.log("Full Response:", response);
                setFolders(response.data.folders);
                setFiles(response.data.files);
                setCurrentFolderPathFromApp(response.data.folderPath);
            } catch (error) {
                console.error("Error fetching folders:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchFolderContents();
    }, [id]);

    const handleFolderClick = (folderId: string) => {
        navigate(`/library/folder/${folderId}`);
    };

    return (
        <div id="library-page" className="w-full">
            {loading && <LoadingAlert />}
            {files.length === 0 && folders.length === 0 && !loading && <NoContentsAlert />}
            {!loading && FolderBreadcrumb()}
            <div id="library-body-content" className="grid grid-cols-2 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-4 2xl:grid-cols-6 gap-4 pt-2 pb-4 pr-4 pl-9">
                {folders.map((folder) => (
                    <AlbumCard
                        key={folder.id}
                        id={folder.id}
                        title={folder.albumName}
                        folderPath={folder.albumPath}
                        onClick={() => {
                            handleFolderClick(folder.id);
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