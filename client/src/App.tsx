import {App as AntApp} from "antd";
import {BrowserRouter} from "react-router-dom";
import {AppRoutes} from "./AppRoutes.tsx";
import {AuthProvider} from "./context/AuthContext.tsx";

export function App() {
    return (
        <AntApp>
            <AuthProvider>
                <BrowserRouter>
                    <AppRoutes/>
                </BrowserRouter>
            </AuthProvider>
        </AntApp>
    );
}
