import {Alert, AlertDescription, AlertTitle} from "@/components/ui/alert.tsx";


export function LoadingAlert() {
    return (
        <div>
            <Alert className="border-0 bg-transparent">
                <AlertTitle>Retrieving Data</AlertTitle>
                <AlertDescription>
                    Loading Folder Contents...
                </AlertDescription>
            </Alert>
        </div>
    )
}