import {AlbumIcon, Image, Settings, Video, LogOut} from "lucide-react"
import {Separator} from "@/components/ui/separator.tsx";
import {
    Sidebar, SidebarContent, SidebarFooter, SidebarGroup, SidebarGroupContent, SidebarGroupLabel, SidebarMenu,
    SidebarMenuButton, SidebarMenuItem,
} from "@/components/ui/sidebar"
import {Avatar, AvatarFallback, AvatarImage} from "@/components/ui/avatar.tsx";

// Menu items.
const NuSidebarItems = [
    {
        title: "Albums",
        url: "/library/albums",
        icon: AlbumIcon,
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