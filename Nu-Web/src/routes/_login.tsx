import {ModeToggle} from "@/components/mode-toggle.tsx";
import {Input} from "@/components/ui/input.tsx";

export function LoginPage() {
    return (
        <div className="h-screen flex flex-col">
            <div id="header" className="text-right p-1">
                 <ModeToggle />
            </div>
            <div id="center" className="flex flex-grow">
                <div className="m-auto">
                    <div id="page-title" className="text-center">
                        <h1 className="text-3xl font-bold mb-12">Login</h1>
                    </div>
                    <div id="username-field" className="mb-4">
                    <Input type="username" placeholder="Username" />
                    </div>
                    <div id="password-field" className="mb-12"></div>
                    <Input type="password" placeholder="Password" />
                </div>
            </div>
        </div>
    );
}