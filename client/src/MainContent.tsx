import {useContext} from "react";
import {LoginPage} from "./components/auth/LoginPage.tsx";
import {AppShell} from "./components/mainPage/AppShell.tsx";
import {AuthContext} from "./context/AuthContext.tsx";
import {UserMainContent} from "./components/mainPage/UserMainContent.tsx";

export function MainContent() {
    const authContext = useContext(AuthContext);
    const {user, isAuthLoading} = authContext;

    const isAuthenticated = user !== undefined;
    if (isAuthLoading) {
        return <></>;
    } else if (!isAuthenticated && !isAuthLoading) {
        return <LoginPage/>
    }

    return <AppShell content={<UserMainContent/>}/>;
}
