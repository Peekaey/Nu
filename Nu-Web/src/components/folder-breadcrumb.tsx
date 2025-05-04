import {
    Breadcrumb,
    BreadcrumbItem,
    BreadcrumbLink,
    BreadcrumbList,
    BreadcrumbSeparator,
} from "@/components/ui/breadcrumb"
import {useGlobalContext} from "@/components/global-context-container.tsx";
import {Fragment} from "react";

export function FolderBreadcrumb() {
    const {currentFolderPath} = useGlobalContext();
    return (
        <div className="pl-9 pt-2">
            <Breadcrumb>
                <BreadcrumbList>
                    {currentFolderPath.map((folder, index) => (
                        <Fragment key={folder.folderName + "-" + index}>
                            {index === 0 ? null : <BreadcrumbSeparator />}
                            {index === 0 ? (
                                <BreadcrumbItem>
                                    <BreadcrumbLink href={`/library/home`}>
                                        {folder.folderName}
                                    </BreadcrumbLink>
                                </BreadcrumbItem>
                            ) : (
                                <BreadcrumbItem>
                                    <BreadcrumbLink href={`/library/folder/${folder.id}`}>
                                        {folder.folderName}
                                    </BreadcrumbLink>
                                </BreadcrumbItem>
                            )}
                        </Fragment>
                    ))}
                </BreadcrumbList>
            </Breadcrumb>
        </div>
    );
}