import {Button, Card, Input, Typography} from "antd";
import {MailOutlined, LockOutlined} from "@ant-design/icons";
import {useContext, useState} from "react";
import {useNavigate} from "react-router-dom";
import {AuthContext} from "../../context/AuthContext.tsx";

export interface LoginFormProps {
    onSubmit: (email: string, password: string) => void;
}

export function LoginForm(props: LoginFormProps) {
    const {isAuthLoading} = useContext(AuthContext);
    const navigate = useNavigate();
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");

    return (
        <div style={{display: "flex", alignItems: "center", justifyContent: "center", height: "100%", width: "100%"}}>
            <Card style={{width: "650px", boxShadow: "rgba(100, 100, 111, 0.2) 0px 7px 29px 0px"}}>
                <Typography.Title style={{textAlign: "center",}}>
                    Iniciar Sesión
                </Typography.Title>
                <Input
                    placeholder="Email"
                    prefix={<MailOutlined/>}
                    style={{marginBottom: "16px"}}
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                <Input.Password
                    placeholder="Contraseña"
                    prefix={<LockOutlined/>}
                    style={{marginBottom: "16px"}}
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                <Button type="primary" onClick={() => props.onSubmit(email, password)}>
                    Iniciar Sesión
                </Button>
                <Button type={"link"} onClick={() => navigate("/signup")} loading={isAuthLoading}>
                    ¿No tenés una cuenta? Creá Clickeando acá
                </Button>
            </Card>
        </div>
    );
}
