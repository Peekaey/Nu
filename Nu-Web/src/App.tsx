import './App.css'
import {LoginPage} from "@/routes/_login.tsx";
import {RegisterPage} from "@/routes/_register.tsx";
import {BrowserRouter, Route, Routes} from "react-router";
import { Toaster } from "@/components/ui/sonner"
import {LibraryHomePage} from "@/routes/library/_home.tsx";
import Layout from "@/layout.tsx";
import {LibraryFoldersPage} from "@/routes/library/_folders.tsx";
import {LibraryImagesPage} from "@/routes/library/_images.tsx";
import {LibraryVideosPage} from "@/routes/library/_videos.tsx";
import {LibrarySettingsPage} from "@/routes/library/_settings.tsx";
import {FolderView} from "@/routes/_folder.tsx";
import {AlbumView} from "@/routes/_album.tsx";
import {GlobalContextProvider} from "@/components/global-context-container.tsx";

function App() {

  return (
    <>
        <Toaster />
        <GlobalContextProvider>
        <BrowserRouter>
            <Routes>
                <Route index element={<LoginPage/>}/>
                <Route path="/login" element={<LoginPage/>}/>
                <Route path="/register" element={<RegisterPage/>}/>
                <Route path="/library" element={<Layout><LibraryHomePage/></Layout>} />
                <Route path="/library/home" element={<Layout><LibraryHomePage/></Layout>} />
                <Route path="/library/folders" element={<Layout><LibraryFoldersPage/></Layout>} />
                <Route path="/library/images" element={<Layout><LibraryImagesPage/></Layout>} />
                <Route path="/library/videos" element={<Layout><LibraryVideosPage/></Layout>} />
                <Route path="/library/folder/:id" element={<Layout><FolderView/></Layout>} />
                <Route path="/library/album/:id" element={<Layout><AlbumView/></Layout>} />
                <Route path="/settings" element={<Layout><LibrarySettingsPage/></Layout>} />
                <Route path="/logout" element={<LoginPage/>}/>
            </Routes>
        </BrowserRouter>
        </GlobalContextProvider>
    </>
  )
}

export default App
