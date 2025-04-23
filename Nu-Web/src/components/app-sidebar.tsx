import { Image, Settings, Video, LogOut, HomeIcon, FolderIcon} from "lucide-react"
import {Separator} from "@/components/ui/separator.tsx";
import {
    Sidebar, SidebarContent, SidebarFooter, SidebarGroup, SidebarGroupContent, SidebarGroupLabel, SidebarMenu,
    SidebarMenuButton, SidebarMenuItem,
} from "@/components/ui/sidebar"
import {Avatar, AvatarFallback, AvatarImage} from "@/components/ui/avatar.tsx";
import {GetRootFolderPath} from "@/utilities/api-manager.ts";
import {useEffect, useState} from "react";


// Menu items.
const NuSidebarItems = [
    {
      title: "Home",
      url: "/library/home",
      icon: HomeIcon,
    },
    {
        title: "Folders",
        url: "/library/folders",
        icon: FolderIcon,
    },
    {
        title: "Images",
        url: "/library/images",
        icon: Image,
    },
    {
        title: "Videos",
        url: "/library/videos",
        icon: Video,
    },
    {
        title: "Settings",
        url: "/settings",
        icon: Settings,
        }
]

export function AppSidebar() {
    const [rootFolderPath, setRootFolderPath] = useState<string>("");

    useEffect(() => {
        // Fetch the root folder path from the server
        // TODO optimise so that it's not calling every single page refresh
        async function fetchRootFolderPath() {
            try {
                const response = await GetRootFolderPath();
                setRootFolderPath(response.data);
            } catch (error) {
                console.error("Error fetching root folder path:", error);
            }
        }

        fetchRootFolderPath();
    });
    return (
        <Sidebar>
            <SidebarContent className="flex flex-col h-full justify-between">
                <div>
                    <SidebarGroup>
                        <SidebarGroupLabel className="flex items-center justify-start gap-1 w-full">
                            <span className="text-2xl">Nu</span>
                        </SidebarGroupLabel>
                        <Separator className="mt-1 mb-1" />
                        <SidebarGroupContent>
                            <SidebarMenu>
                                {NuSidebarItems.map((item) => (
                                    <SidebarMenuItem key={item.title}>
                                        <SidebarMenuButton asChild>
                                            <a href={item.url}>
                                                <item.icon />
                                                <span className="text-base">{item.title}</span>
                                            </a>
                                        </SidebarMenuButton>
                                    </SidebarMenuItem>
                                ))}
                            </SidebarMenu>
                        </SidebarGroupContent>
                    </SidebarGroup>
                </div>
                <SidebarFooter className="mt-auto">
                    <div className="flex flex-wrap text-sm text-gray-500">
                        <span className="flex-shrink-0">Root Storage Path:&nbsp;</span>
                        <span id="root-folder-path" className="flex-1 break-all">{rootFolderPath}</span>
                    </div>
                    <Separator className="mt-1 mb-1" />
                    <div className="flex items-center justify-between w-full">
                        <Avatar>
                            <AvatarImage src="https://avatars.githubusercontent.com/u/64738072?v=4" />
                            <AvatarFallback>CN</AvatarFallback>
                        </Avatar>
                        <button className="p-2 rounded-full hover:bg-sidebar-accent">
                            <LogOut size={18} />
                        </button>
                    </div>
                </SidebarFooter>
            </SidebarContent>
        </Sidebar>
    )
}