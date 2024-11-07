import {Navigate, Route, Routes} from "react-router-dom";
import {useContext} from "react";
import {AuthContext} from "./context/AuthContext.tsx";
import {MainContent} from "./MainContent.tsx";
import {LoginPage} from "./components/auth/LoginPage.tsx";
import {SignUpPage} from "./components/auth/SignUpPage.tsx";

export function AppRoutes() {
    const {user, isInitialAuthLoading} = useContext(AuthContext);
    const isAuthenticated = user !== undefined;

    return (
        <Routes>
            <Route path="/" element={isAuthenticated ? <MainContent/> : <Navigate to={"/login"}/>}/>
            <Route path="/login"
                   element={isAuthenticated ? <Navigate to="/"/> : (isInitialAuthLoading ? <></> : <LoginPage/>)}/>
            <Route path="/signup"
                   element={isAuthenticated ? <Navigate to="/"/> : (isInitialAuthLoading ? <></> : <SignUpPage/>)}/>
        </Routes>
    );
}
