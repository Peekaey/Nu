import {useState, MouseEvent, useEffect} from 'react';

export function ImageCard({ src, alt, title, description }: { src: string; alt: string; title: string; description: string }) {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isZoomed, setIsZoomed] = useState(false);
    const [isDragging, setIsDragging] = useState(false);
    const [dragStart, setDragStart] = useState({ x: 0, y: 0 });
    const [currentPosition, setCurrentPosition] = useState({ x: 0, y: 0 });
    const [hasMovedDuringDrag, setHasMovedDuringDrag] = useState(false);

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

    const handleImageClick = (e: MouseEvent) => {
        e.stopPropagation();

        if (!isModalOpen) {
            setIsModalOpen(true);
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
                    src={src.startsWith("data:") ? src : `data:image/jpeg;base64,${src}`}
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
                    <div className="w-[150vw] h-[150vh] absolute inset-0 -translate-x-1/4 -translate-y-1/4"
                         style={{
                             transform: `scale(${isZoomed ? 2 : 1}) translate(${currentPosition.x}px, ${currentPosition.y}px)`,
                             transition: isDragging ? 'none' : 'transform 0.1s ease-out'
                         }}
                    >
                        <div className="absolute inset-0 bg-black/50 backdrop-blur-sm" />
                    </div>

                    <div className="relative z-10 p-10">
                        <button
                            onClick={closeModal}
                            className="absolute top-4 right-4 rounded-full p-2 cursor-pointer text-white"
                        >
                            ✕
                        </button>
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
                                src={src.startsWith("data:") ? src : `data:image/jpeg;base64,${src}`}
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