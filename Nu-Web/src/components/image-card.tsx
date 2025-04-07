
export function ImageCard({ src, alt, title, description }: { src: string; alt: string; title: string; description: string }) {
    return (
        <div className="flex flex-col items-center p-4 border rounded-lg shadow-md">
            <img src={src} alt={alt} className="w-full h-64 object-cover rounded-lg" />
            <h2 className="mt-2 text-sm font-semibold">{title}</h2>
            <p className="mt-1 text-sm text-gray-600">{description}</p>
        </div>
    );
}