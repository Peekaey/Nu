
export interface LibraryFolderFilesResponseDTO {
    id: string;
    fileName: string;
    filePath: string;
    fileType: string;
    fileSize: number;
    fileCreatedAt: string;
    imageData: string;
    serverImagePath: string;
}

export interface LibraryFolderResponseDTO {
    id: string;
    title: string;
    folderPath: string;
    onClick: (id: string) => void;
}

export interface LibraryFolderContentsResponseDTO {
    folders: LibraryFolderResponseDTO[];
    files: LibraryFolderFilesResponseDTO[];
    folderPath: LibraryFolderPathChunk[];
}

export interface LibraryFolderPathChunk{
    id: string;
    folderName: string;
}