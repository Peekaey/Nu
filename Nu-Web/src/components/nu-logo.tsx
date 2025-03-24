
export function NuLogo() {
    return (
        <svg
            viewBox="0 0 200 200"
            xmlns="http://www.w3.org/2000/svg"
            className="h-10 w-10" // Increased to h-10 w-10 (2.5rem or 40px)
        >
            <line x1="60" y1="50" x2="60" y2="150" stroke="currentColor" strokeWidth="4" />
            <line x1="60" y1="50" x2="140" y2="150" stroke="currentColor" strokeWidth="4" />
            <line x1="140" y1="50" x2="140" y2="150" stroke="currentColor" strokeWidth="4" />
        </svg>
    )
}