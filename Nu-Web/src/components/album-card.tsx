import { Folder } from "lucide-react"
import {LibraryFolderResponseDTO} from "@/types/folders.ts";


export function AlbumCard({ id, title, folderPath, onClick }: LibraryFolderResponseDTO) {
    return (
        <div
            id={id}
            className="flex flex-col items-center justify-center p-4 border rounded-lg shadow-md cursor-pointer"
            onClick={() => onClick(id)}
        >
            <div className="w-full h-16 flex items-center justify-center rounded-lg">
                <Folder size={64} className="text-gray-400" />
            </div>
            <h2 className="mt-2 text-sm font-semibold text-center break-words w-full">{title}</h2>
            <p
                className="mt-1 text-sm text-gray-600 text-center break-words w-full"
                style={{ wordWrap: "break-word", overflowWrap: "break-word" }}
            >
                {folderPath}
            </p>
        </div>
    );
}