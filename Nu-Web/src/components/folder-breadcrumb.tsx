import {
    Breadcrumb,
    BreadcrumbItem,
    BreadcrumbLink,
    BreadcrumbList,
    BreadcrumbPage,
    BreadcrumbSeparator,
} from "@/components/ui/breadcrumb"
import {useGlobalContext} from "@/components/global-context-container.tsx";
import {Fragment} from "react";

export function FolderBreadcrumb() {
    const {currentFolderPath} = useGlobalContext();
    let {rootFolderPath} = useGlobalContext();
    // Convert all \ to / in the root folder path
    // TODO Look at normalising this some other way
    const currentFolderPathArray = currentFolderPath.split("\\");
    const rootFolderPathArray = rootFolderPath.split("\\");
    rootFolderPath = rootFolderPathArray.join("/");
    const relativePath = currentFolderPath.replace(rootFolderPath, "").replace(/^\//, "");

    return (
        <div className="pl-9 pt-2">
        <Breadcrumb>
            <BreadcrumbList>
                <BreadcrumbItem>
                    <BreadcrumbLink href="/library/home">Root</BreadcrumbLink>
                </BreadcrumbItem>

                {relativePath.split("/").map((folder, index) => (
                    <Fragment key={folder + index}>
                        <BreadcrumbSeparator/>
                        <BreadcrumbItem>
                            <BreadcrumbLink href={`/library/folder/${folder}`}>
                                {folder}
                            </BreadcrumbLink>
                        </BreadcrumbItem>
                    </Fragment>
                ))}
            </BreadcrumbList>
        </Breadcrumb>
        </div>
    )
}