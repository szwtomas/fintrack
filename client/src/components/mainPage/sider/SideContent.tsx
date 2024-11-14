import {Divider, Menu} from "antd";
import {
    HomeOutlined,
    SettingOutlined,
    LogoutOutlined,
} from "@ant-design/icons";
import {useContext} from "react";
import {AuthContext} from "../../../context/AuthContext.tsx";

export interface SideContentProps {
    userRole: "counselor" | "consultant" | "admin";
}

export function SideContent() {
    const {logOut} = useContext(AuthContext);

    const sideItems = [
        {
            key: "home",
            icon: <HomeOutlined/>,
            label: "Inicio",
        },
        {
            key: "configuration",
            icon: <SettingOutlined/>,
            label: "Configuración",
        },
        {
            key: "logout",
            icon: <LogoutOutlined/>,
            label: "Cerrar Sesión",
            onClick: logOut,
        }
    ];


    return (
        <div>
            <div style={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                height: "70px"
            }}>
                <h1 style={{fontSize: "26px"}}>FinTrack</h1>
            </div>

            <Menu
                theme="light"
                mode="inline"
                defaultSelectedKeys={['4']}
                items={sideItems}
            />
            <Divider/>
        </div>
    );
}