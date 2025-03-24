import {ModeToggle} from "@/components/mode-toggle.tsx";
import {useForm} from "react-hook-form";
import { z } from "zod";
import {zodResolver} from "@hookform/resolvers/zod";
import { Button } from "@/components/ui/button"
import {Form, FormControl, FormField, FormItem, FormMessage,} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import {SendUserRegistrationRequest} from "@/utilities/api-manager.ts";
import { toast } from "sonner"
import { useNavigate } from "react-router";

const formSchema = z.object({
    // TODO Add Client Side SQL Injection Protection
    // TODO Add Second Confirm Password Field
    username: z.string().email({
        message: "Please enter a valid email address"
    }).max(50, {
        message: "Email must be less than 50 characters",
    }),
    password: z.string()
        .min(8, { message: "Password must be at least 8 characters" })
        .max(50, { message: "Password must be less than 50 characters" })
        .regex(/[A-Z]/, { message: "Password must contain at least one uppercase letter" })
        .regex(/[0-9]/, { message: "Password must contain at least one number" })
        .regex(/[^A-Za-z0-9]/, { message: "Password must contain at least one symbol" }),
})

export function RegisterPage() {

    const navigate = useNavigate();

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            username: "",
            password: "",
        }
    })

    async function onSubmit(values: z.infer<typeof formSchema>) {
        const userRegistrationRequest = {
            Email: values.username, // Map username (which is validated as email) to Email
            Password: values.password
        }
        try {
            const response = await SendUserRegistrationRequest(userRegistrationRequest);

            if (response.status === 200) {
                toast("Account Created Successfully! Redirecting To Login Page...");
                navigate("/login");
            } else if (response.status === 409) {
                toast("An account with that email address already exists. Please try again with a different email address.");
            } else {
                toast("An error occurred while creating your account. Please try again later.");

            }
        } catch (error) {
            console.error(error);
        }
    }

    return (
        <div className="h-screen flex flex-col dark:bg-black-500">
            <div id="header" >
                <div className="text-center">
                </div>
                <div className="text-right mt-4 mr-2">
                    <ModeToggle />
                </div>
            </div>
            <div id="center" className="flex flex-grow">
                <div className="m-auto">
                    {/* Acrylic Card Wrapper */}
                    <Form {...form}>
                        <form onSubmit={form.handleSubmit(onSubmit)}>
                            <div className="bg-white/30 dark:bg-black/30 backdrop-blur-md rounded-xl p-8 shadow-lg">
                                <div id="page-title" className="text-center content-center">
                                    <h1 className="text-3xl font-bold mb-12">Register Account</h1>
                                </div>
                                <FormField
                                    control={form.control}
                                    name="username"
                                    render={({ field }) => (
                                        <FormItem>
                                            <FormMessage />
                                            <FormControl>
                                                <div id="username-field" className="mb-4">
                                                    <Input type="text" placeholder="Username" className="w-80" {...field} />
                                                </div>
                                            </FormControl>
                                        </FormItem>
                                    )}
                                />
                                <FormField
                                    control={form.control}
                                    name="password"
                                    render={({ field }) => (
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
                                <div id="submit-button cursor-pointer" className="mt-4">
                                    <Button type="submit">Create Account</Button>
                                </div>
                            </div>
                        </form>
                    </Form>
                </div>
            </div>
        </div>
    );
}