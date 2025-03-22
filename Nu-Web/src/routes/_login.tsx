import {ModeToggle} from "@/components/mode-toggle.tsx";
import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";

export function LoginPage() {
    return (
        <div className="h-screen flex flex-col dark:bg-black-500">
            <div id="header" className="text-right p-1">
                <ModeToggle />
            </div>
            <div id="center" className="flex flex-grow">
                <div className="m-auto">
                    {/* Acrylic Card Wrapper */}
                    <div className="bg-white/30 dark:bg-black/30 backdrop-blur-md rounded-xl p-8 shadow-lg">
                        <div id="page-title" className="text-center content-center">
                            <h1 className="text-3xl font-bold mb-12">Login To Nu</h1>
                        </div>
                        <div id="username-field" className="mb-4">
                            <Input type="username" placeholder="Username" className="w-80" />
                        </div>
                        <div id="password-field" className="mb-4">
                            <Input type="password" placeholder="Password" className="w-80" />
                        </div>
                        <div id="register-button" className="mb-4 text-right">
                            <button className="text-blue-500 hover:underline text-sm">
                                Register
                            </button>
                        </div>
                        <div id="submit-button" className="mt-4">
                            <Button>Sign In</Button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}