import {useContext} from "react";
import {AuthContext} from "../../context/AuthContext.tsx";
import {Layout, notification} from "antd";
import {Content, Footer} from "antd/es/layout/layout";
import {SignUpForm} from "./SignUpForm.tsx";
import {useNavigate} from "react-router-dom";

export function SignUpPage() {
    const [api, contextHolder] = notification.useNotification();
    const {signUp} = useContext(AuthContext);
    const navigate = useNavigate();

    function showErrorNotification(message: string) {
        api["error"]({
            message: "Error al registrarse",
            description: message,
        });
    }

    async function onSignUpSubmit(email: string, firstName: string, lastName: string, password: string) {
        try {
            console.log(`${firstName} ${lastName}`);
            const signUpErrorMessage = await signUp(email, password);
            if (signUpErrorMessage !== undefined) {
                showErrorNotification(signUpErrorMessage);
            } else {
                navigate("/");
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
                        <SignUpForm onSignUpSubmit={onSignUpSubmit}/>
                    </Content>
                </Layout>
                <Footer style={{textAlign: "center", backgroundColor: "white"}}>
                    FinTrack ©{new Date().getFullYear()} Created by Tomás Szwarcberg
                </Footer>
            </Layout>
        </>
    );
}
