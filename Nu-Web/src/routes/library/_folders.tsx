import {AlbumCard} from "@/components/album-card.tsx";
import {useEffect, useState} from "react";
import {GetAllLibraryAlbums} from "@/utilities/api-manager.ts";
import {LibraryAlbumsResponseDTO} from "@/types/library.ts";
import { useNavigate } from "react-router";

export function LibraryFoldersPage() {
    const [albums, setAlbums] = useState<LibraryAlbumsResponseDTO[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        // Fetch Initial Folders
        async function fetchAlbums() {
            try {
                const response = await GetAllLibraryAlbums();
                setAlbums(response.data);
            } catch (error) {
                console.error("Error fetching albums:", error);
            }
        }

        fetchAlbums();
    }, []);

    const handleAlbumClick = (id: string) => {
        navigate(`/library/album/${id}`);
    };

    return (
        <div id="library-page" className="w-full">
            <div id="library-body-content" className="grid grid-cols-2 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-4 2xl:grid-cols-6 gap-4 p-4">
                {albums.map((album) => (
                    <AlbumCard
                        key ={album.id}
                        id={album.id}
                        title={album.albumName}
                        folderPath={album.albumPath}
                        onClick={handleAlbumClick}
                    />
                ))}
            </div>
        </div>
    );
}