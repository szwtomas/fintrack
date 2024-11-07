import {Button, Card, Input, Typography} from "antd";
import {LockOutlined, MailOutlined, UserOutlined} from "@ant-design/icons";
import {useNavigate} from "react-router-dom";
import {useContext, useState} from "react";
import {AuthContext} from "../../context/AuthContext.tsx";

export interface SignUpFormProps {
    onSignUpSubmit: (email: string, firstName: string, lastName: string, password: string) => Promise<void>;
}

export function SignUpForm(props: SignUpFormProps) {
    const navigate = useNavigate();
    const {isAuthLoading} = useContext(AuthContext);
    const [email, setEmail] = useState<string>("");
    const [firstName, setFirstName] = useState<string>("");
    const [lastName, setLastName] = useState<string>("");
    const [password, setPassword] = useState<string>("");

    const [emailError, setEmailError] = useState<boolean>(false);
    const [firstNameError, setFirstNameError] = useState<boolean>(false);
    const [lastNameError, setLastNameError] = useState<boolean>(false);
    const [passwordError, setPasswordError] = useState<boolean>(false);

    const onCreateAccountClick = async () => {
        setEmailError(email === "");
        setFirstNameError(firstName === "");
        setLastNameError(lastName === "");
        setPasswordError(password === "");

        const signUpFormErrors = email === "" || firstName === "" || lastName === "" || password === "";
        if (signUpFormErrors) {
            return;
        }

        await props.onSignUpSubmit(email, firstName, lastName, password);
    }

    return (
        <div style={{display: "flex", alignItems: "center", justifyContent: "center", height: "100%", width: "100%"}}>
            <Card style={{width: "500px", boxShadow: "rgba(100, 100, 111, 0.2) 0px 7px 29px 0px"}}>
                <Typography.Title style={{textAlign: "center",}}>
                    Crear Cuenta
                </Typography.Title>
                <Input
                    status={emailError ? "error" : ""}
                    placeholder="Email"
                    prefix={<MailOutlined/>}
                    style={{marginBottom: "16px"}}
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                <Input
                    status={firstNameError ? "error" : ""}
                    placeholder="Nombre"
                    prefix={<UserOutlined/>}
                    style={{marginBottom: "16px"}}
                    value={firstName}
                    onChange={(e) => setFirstName(e.target.value)}
                />
                <Input
                    status={lastNameError ? "error" : ""}
                    placeholder="Apellido"
                    prefix={<UserOutlined/>}
                    style={{marginBottom: "16px"}}
                    value={lastName}
                    onChange={(e) => setLastName(e.target.value)}
                />
                <Input.Password
                    placeholder="Contraseña"
                    prefix={<LockOutlined/>}
                    style={{marginBottom: "16px"}}
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    status={passwordError ? "error" : ""}
                />
                <Button type="primary" onClick={onCreateAccountClick} loading={isAuthLoading}>
                    Crear Cuenta
                </Button>
                <Button type={"link"} onClick={() => navigate("/login")}>
                    ¿Ya tenés una cuenta? Inicia Sesión acá
                </Button>
            </Card>
        </div>
    );
}