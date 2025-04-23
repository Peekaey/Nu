import {Alert, AlertDescription, AlertTitle} from "@/components/ui/alert.tsx";



export function NoContentsAlert() {
    return (
        <div>
            <Alert className="border-0 bg-transparent">
                <AlertTitle>No Content Found</AlertTitle>
                <AlertDescription>
                    No content found in this folder.
                </AlertDescription>
            </Alert>
        </div>
    );
}