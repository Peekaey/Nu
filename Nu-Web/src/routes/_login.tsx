import {ModeToggle} from "@/components/mode-toggle.tsx";
import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import { useNavigate } from "react-router";
import {useForm} from "react-hook-form";
import {z} from "zod";
import {zodResolver} from "@hookform/resolvers/zod";
import {SendUserAuthenticationRequest} from "@/utilities/api-manager.ts";
import {toast} from "sonner";
import {UserAuthRequestDTO} from "@/types/user-registration.ts";
import {Form, FormControl, FormField, FormItem, FormMessage} from "@/components/ui/form.tsx";


const formSchema = z.object({
    // TODO Add Client Side SQL Injection Protection
    username: z.string().min(1, { message: "Username is required" }),
    password: z.string().min(1, { message: "Password is required" }),
})

export function LoginPage() {

    const navigate = useNavigate();

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            username: "",
            password: "",
        }
    })

    async function onSubmit(values: z.infer<typeof formSchema>) {
        const userLoginRequest : UserAuthRequestDTO = {
            Email: values.username,
            Password: values.password
        }
        try {
            const response = await SendUserAuthenticationRequest(userLoginRequest);

            if (response.status === 200) {
                toast("Login Successful! Redirecting To Library...");
                navigate("/library");
            }

            if (response.status === 401) {
                toast("Invalid username or password. Please try again.");
            }

            if (response.status === 400) {
                toast("Invalid inputs. Please check your username and password.");
            }

            if (response.status === 500) {
                toast("An error occurred while authenticating with the system. Please try again later.");
            }

        } catch (error) {
            console.error(error);
        }
    }

    return (
        <div className="h-screen flex flex-col dark:bg-black-500">
            <div id="header" className="text-right mt-4 mr-4">
                <ModeToggle />
            </div>
            <div id="center" className="flex flex-grow">
                <div className="m-auto">
                    {/* Acrylic Card Wrapper */}
                    <Form {...form}>
                        <form onSubmit={form.handleSubmit(onSubmit)}>
                            <div className="bg-white/30 dark:bg-black/30 backdrop-blur-md rounded-xl p-8 shadow-lg">
                                <div>

                                </div>
                                <div id="page-title" className="text-center content-center">
                                    <h1 className="text-3xl font-bold mb-12">Login To Nu</h1>
                                </div>
                                <FormField
                                    control={form.control}
                                    name="username"
                                    render = {({ field }) => (
                                        <FormItem>
                                            <FormMessage />
                                            <FormControl>
                                                <div id="username-field" className="mb-4">
                                                    <Input type="username" placeholder="Username" className="w-80" {...field} />
                                                </div>
                                            </FormControl>
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="password"
                                    render = {({ field }) => (
                                        <FormItem>
                                            <FormMessage />
                                            <FormControl>
                                                <div id="password-field" className="mb-4">
                                                    <Input type="password" placeholder="Password" className="w-80" {...field} />
                                                </div>
                                            </FormControl>
                                        </FormItem>
                                    )}
                                />
                                <div id="register-button" className="mb-4 text-right">
                                    <button className="text-blue-500 hover:underline text-sm cursor-pointer" onClick={() => navigate("/register")}>
                                        Register
                                    </button>
                                </div>

                                <div id="submit-button" className="mt-4">
                                    <Button className="cursor-pointer" type="submit">Sign In</Button>
                                </div>
                            </div>
                        </form>
                    </Form>
                </div>
            </div>
        </div>
    );
}