import {App as AntApp} from "antd";
import {BrowserRouter} from "react-router-dom";
import {AppRoutes} from "./AppRoutes.tsx";

export function App() {
    return (
        <AntApp>
            <BrowserRouter>
                <AppRoutes/>
            </BrowserRouter>
        </AntApp>
    );
}
