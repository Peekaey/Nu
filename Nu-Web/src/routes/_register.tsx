import {ModeToggle} from "@/components/mode-toggle.tsx";

import {useForm} from "react-hook-form";
import { z } from "zod";
import {zodResolver} from "@hookform/resolvers/zod";

import { Button } from "@/components/ui/button"
import {Form, FormControl, FormField, FormItem, FormMessage,} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import {SendUserRegistrationRequest} from "@/utilities/api-manager.ts";


const formSchema = z.object({
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

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            username: "",
            password: "",
        }
    })

    function onSubmit(values: z.infer<typeof formSchema>) {
        const userRegistrationRequest = {
            Email: values.username, // Map username (which is validated as email) to Email
            Password: values.password
        }
        SendUserRegistrationRequest(userRegistrationRequest);
    }

    return (
        <div className="h-screen flex flex-col dark:bg-black-500">
            <div id="header" className="text-right p-1">
                <ModeToggle />
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
                                <div id="submit-button" className="mt-4">
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