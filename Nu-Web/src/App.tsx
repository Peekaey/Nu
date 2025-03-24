import './App.css'
import {LoginPage} from "@/routes/_login.tsx";
import {RegisterPage} from "@/routes/_register.tsx";
import {BrowserRouter, Route, Routes} from "react-router";
import { Toaster } from "@/components/ui/sonner"
import {LibraryPage} from "@/routes/_library.tsx";
import Layout from "@/layout.tsx";

function App() {

  return (
    <>
        <Toaster />
        <BrowserRouter>
            <Routes>
                <Route index element={<LoginPage/>}/>
                <Route path="/login" element={<LoginPage/>}/>
                <Route path="/register" element={<RegisterPage/>}/>
                <Route path="/library" element={<Layout><LibraryPage/></Layout>} />
            </Routes>
        </BrowserRouter>
    </>
  )
}

export default App
