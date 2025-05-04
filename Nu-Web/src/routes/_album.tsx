
import { useParams } from "react-router-dom";
import {LibraryFolderFilesResponseDTO} from "@/types/folders.ts";
import {ImageCard} from "@/components/image-card.tsx";
import {useEffect, useState} from "react";
import {GetLibraryAlbumContents} from "@/utilities/api-manager.ts";
import {LoadingAlert} from "@/components/loading-alert.tsx";
import {NoContentsAlert} from "@/components/no-contents-alert.tsx";

export function AlbumView() {
    const { id } = useParams();
    const [files, setFiles] = useState<LibraryFolderFilesResponseDTO[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (!id) {
            console.error("No folder id provided in the URL.");
            return;
        }

        async function fetchFiles() {
            try {
                const response = await GetLibraryAlbumContents(id);
                setFiles(response.data);
            } catch (error) {
                console.error("Error fetching files:", error);
            } finally {
                setLoading(false);
            }
        }

        fetchFiles();
    }, [id]);

    return (
        <div id="library-page" className="w-full">
            {loading && <LoadingAlert />}
            <div id="library-body-content" className="grid grid-cols-2 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-4 2xl:grid-cols-6 gap-4 p-4">
                {files.length === 0 && !loading && <NoContentsAlert/>}

                {files.map((file, index) => (
                    <ImageCard
                        key={index}
                        id={file.id}
                        src={file.serverImagePath}
                        alt={"Image"}
                        title={file.fileName || "Untitled"}
                        description={file.filePath || "No description available"}
                    />
                ))}
            </div>
        </div>
    );
}