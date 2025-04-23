import { SidebarTrigger } from "@/components/ui/sidebar-trigger";
import { Search } from "lucide-react";
import { Input } from "@/components/ui/input";
import { ModeToggle } from "@/components/mode-toggle";

export function Header() {
    return (
        <div id="header" className="relative w-full flex items-center justify-between mt-4">
            <div className="relative w-3/4 flex items-center gap-2">
                <SidebarTrigger className="shrink-0" />
                <div className="relative w-full">
                    <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground pointer-events-none" />
                    <Input
                        type="search"
                        placeholder="Search..."
                        className="pl-9 w-full"
                    />
                </div>
            </div>
            <div className="w-1/4 flex justify-end mr-4">
                <ModeToggle />
            </div>
        </div>
    );
}