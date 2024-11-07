import {Layout, notification} from "antd";
import {Content, Footer} from "antd/es/layout/layout";
import {LoginForm} from "./LoginForm";
import {useContext} from "react";
import {AuthContext} from "../../context/AuthContext.tsx";

export function LoginPage() {
    const [api, contextHolder] = notification.useNotification();
    const {logIn} = useContext(AuthContext);

    function showErrorNotification(message: string) {
        api["error"]({
            message: "Error al registrarse",
            description: message,
        });
    }

    async function onLoginSubmit(email: string, password: string): Promise<void> {
        try {
            const logInError = await logIn(email, password);
            if (logInError) {
                showErrorNotification(logInError);
            }
        } catch (err) {
            console.error("Error logging in", err);
        }
    }

    return (
        <>
            {contextHolder}
            <Layout
                style={{borderRadius: 8, overflow: "hidden", width: "100vw", height: "100vh", padding: 0, margin: 0}}>
                <Layout>
                    <Content style={{height: "100%", backgroundColor: "white"}}>
                        <LoginForm onSubmit={onLoginSubmit}/>
                    </Content>
                </Layout>
                <Footer style={{textAlign: "center", backgroundColor: "white"}}>
                    OV ©{new Date().getFullYear()} Created by Tomás Szwarcberg
                </Footer>
            </Layout>
        </>
    );
}
