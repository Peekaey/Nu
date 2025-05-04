import {useState, MouseEvent, useEffect} from 'react';
import {DownloadImage, GetImageData} from "@/utilities/api-manager.ts";

export function ImageCard({ id, src, alt, title, description }: { id: string; src: string; alt: string; title: string; description: string }) {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isZoomed, setIsZoomed] = useState(false);
    const [isDragging, setIsDragging] = useState(false);
    const [dragStart, setDragStart] = useState({ x: 0, y: 0 });
    const [currentPosition, setCurrentPosition] = useState({ x: 0, y: 0 });
    const [hasMovedDuringDrag, setHasMovedDuringDrag] = useState(false);
    const [imageData, setImageData] = useState<string>("");

    // Stops background scrolling when modal open
    useEffect(() => {
        if (isModalOpen) {
            document.body.style.overflow = 'hidden';
        } else {
            document.body.style.overflow = 'unset';
        }

        return () => {
            document.body.style.overflow = 'unset';
        };
    }, [isModalOpen]);

    const handleImageClick = async (e: MouseEvent) => {
        e.stopPropagation();

        if (!isModalOpen) {
            try {
                const response = await GetImageData(id);
                setImageData(response.data);
                setIsModalOpen(true);
            } catch (error) {
                console.error("Error fetching image data:", error);
            }
            return;
        }

        if (!hasMovedDuringDrag) {
            if (!isZoomed) {
                setIsZoomed(true);
            } else {
                setIsZoomed(false);
                setCurrentPosition({ x: 0, y: 0 });
            }
        }
    };

    const closeModal = () => {
        setIsModalOpen(false);
        setIsZoomed(false);
        setCurrentPosition({ x: 0, y: 0 });
    };

    const handleMouseDown = (e: MouseEvent) => {
        if (!isZoomed) return;
        e.stopPropagation();
        setIsDragging(true);
        setHasMovedDuringDrag(false);
        setDragStart({
            x: e.clientX - currentPosition.x,
            y: e.clientY - currentPosition.y
        });
    };

    const handleMouseMove = (e: MouseEvent) => {
        if (isDragging && isZoomed) {
            setHasMovedDuringDrag(true);
            setCurrentPosition({
                x: e.clientX - dragStart.x,
                y: e.clientY - dragStart.y
            });
        }
    };

    const handleMouseUp = () => {
        setIsDragging(false);
        setTimeout(() => {
            setHasMovedDuringDrag(false);
        }, 100);
    };

    return (
        <>
            <div className="flex flex-col items-center p-4 border rounded-lg shadow-md">
                <img
                    // TODO Stop Hardcoding IP Addresses
                    src={"http://192.168.1.165:5000/" + src}
                    alt={alt}
                    className="w-full h-64 object-cover rounded-lg cursor-pointer"
                    onClick={handleImageClick}
                />
                <h2 className="mt-2 text-sm font-semibold text-center break-words w-full">
                    {title}
                </h2>
                <p className="mt-1 text-sm text-gray-600 text-center break-words w-full">
                    {description}
                </p>
            </div>

            {isModalOpen && (
                <div
                    className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 backdrop-blur-sm"
                    onClick={closeModal}
                >
                    <div className="absolute top-6 left-4 flex items-center gap-4 z-[60]">
                        <h2 className="text-white text-md">
                            {title}
                        </h2>
                    </div>
                    <div className="absolute top-4 right-4 flex items-center gap-4 z-[60]">
                        <button
                            onClick={async (e) => {
                                e.preventDefault();
                                e.stopPropagation();
                                try {
                                    const response = await DownloadImage(id);

                                    // Get the MIME type from response headers
                                    const mimeType = response.headers['content-type'];

                                    // Get file extension from MIME type
                                    const extension = mimeType.split('/')[1] || '';

                                    // Create blob with correct MIME type
                                    const blob = new Blob([response.data], { type: mimeType });
                                    const blobUrl = window.URL.createObjectURL(blob);

                                    // Create temporary link and trigger download
                                    const link = document.createElement('a');
                                    link.href = blobUrl;
                                    link.download = `${title}.${extension}`; // Add appropriate extension
                                    document.body.appendChild(link);
                                    link.click();

                                    // Cleanup
                                    document.body.removeChild(link);
                                    window.URL.revokeObjectURL(blobUrl);

                                } catch (error) {
                                    console.error("Error fetching image data:", error);
                                }
                            }}
                            className="rounded-full p-2 cursor-pointer text-white hover:bg-white/10"
                            type="button"
                        >
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6">
                                <path strokeLinecap="round" strokeLinejoin="round" d="M3 16.5v2.25A2.25 2.25 0 0 0 5.25 21h13.5A2.25 2.25 0 0 0 21 18.75V16.5M16.5 12 12 16.5m0 0L7.5 12m4.5 4.5V3" />
                            </svg>
                        </button>
                        <button
                            onClick={(e) => {
                                e.stopPropagation();
                                if (isZoomed) {
                                    setIsZoomed(false);
                                    setCurrentPosition({ x: 0, y: 0 });
                                } else {
                                    setIsZoomed(true);
                                }
                            }}
                            className="rounded-full p-2 cursor-pointer text-white hover:bg-white/10"
                            type="button"
                        >
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-6 h-6">
                                {isZoomed ? (
                                    <path strokeLinecap="round" strokeLinejoin="round" d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607ZM13.5 10.5h-6" />
                                ) : (
                                    <path strokeLinecap="round" strokeLinejoin="round" d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607ZM10.5 7.5v6m3-3h-6" />
                                )}
                            </svg>
                        </button>
                        <button
                            onClick={closeModal}
                            className="rounded-full p-2 cursor-pointer text-white hover:bg-white/10"
                            type="button"
                        >
                            ✕
                        </button>
                    </div>

                    <div className="w-[150vw] h-[150vh] absolute inset-0 -translate-x-1/4 -translate-y-1/4"
                         style={{
                             transform: `scale(${isZoomed ? 2 : 1}) translate(${currentPosition.x}px, ${currentPosition.y}px)`,
                             transition: isDragging ? 'none' : 'transform 0.1s ease-out'
                         }}
                    >
                        <div className="absolute inset-0 bg-black/50 backdrop-blur-sm" />
                    </div>

                    <div className="relative z-10 p-10">
                        <div
                            className={`relative ${isZoomed ? 'cursor-move' : 'cursor-zoom-in'}`}
                            onClick={handleImageClick}
                            onMouseDown={handleMouseDown}
                            onMouseMove={handleMouseMove}
                            onMouseUp={handleMouseUp}
                            onMouseLeave={handleMouseUp}
                            style={{
                                transform: `scale(${isZoomed ? 2 : 1}) translate(${currentPosition.x}px, ${currentPosition.y}px)`,
                                transition: isDragging ? 'none' : 'transform 0.1s ease-out'
                            }}
                        >
                            <img
                                // TODO Stop Hardcoding IP Addresses
                                src={"http://192.168.1.165:5000/" + imageData}
                                alt={alt}
                                className="max-w-full max-h-[calc(100vh-5rem)] rounded-lg object-contain"
                                draggable="false"
                            />
                        </div>
                    </div>
                </div>
            )}
        </>
    );
}