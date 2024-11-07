import React, {Context, createContext, useEffect, useState} from "react";
import * as authApi from "../api/authApi.ts";
import {InvalidCredentialsException} from "../api/authApi.ts";
import {LogInInvalidCredentialsError, LogInUnexpectedError, SignUpUnexpectedError} from "../errors/errors.ts";

export interface User {
    userId: number;
    email: string;
}

interface AuthContextProps {
    user?: User;
    logIn: (email: string, password: string) => Promise<string | undefined>;
    logOut: () => Promise<void>;
    signUp: (email: string, password: string) => Promise<string | undefined>;
    isInitialAuthLoading: boolean;
    isAuthLoading: boolean;
}

const initialAuthContext: AuthContextProps = {
    user: undefined,
    logIn: async () => undefined,
    logOut: async () => undefined,
    signUp: async () => undefined,
    isAuthLoading: false,
    isInitialAuthLoading: false,
};

export const AuthContext: Context<AuthContextProps> = createContext<AuthContextProps>(initialAuthContext);

interface AuthProviderProps {
    children: React.ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = (props) => {
    const [user, setUser] = useState<User | undefined>(undefined);
    const [initialAuthLoading, setInitialAuthLoading] = useState<boolean>(false);
    const [authLoading, setAuthLoading] = useState<boolean>(false);

    async function logIn(email: string, password: string): Promise<string | undefined> {
        try {
            setAuthLoading(true);
            const userAuthenticated = await authApi.logIn(email, password);
            setUser(userAuthenticated);
        } catch (err) {
            console.error("Error logging in", err);
            if (err instanceof InvalidCredentialsException) {
                return LogInInvalidCredentialsError;
            } else {
                return LogInUnexpectedError;
            }
        } finally {
            setAuthLoading(false);
        }
    }

    async function logOut(): Promise<void> {
        try {
            await authApi.logOut();
            setUser(undefined);
        } catch (err) {
            console.error("Error logging out", err);
        }
    }

    async function signUp(email: string, password: string): Promise<string | undefined> {
        try {
            setAuthLoading(true);
            await authApi.signUp(email, password);
            await logIn(email, password);
            return undefined;
        } catch (err) {
            console.error(err);
            return SignUpUnexpectedError;
        } finally {
            setAuthLoading(false);
        }
    }

    useEffect(() => {
        setInitialAuthLoading(true);

        async function checkIsAuthenticated(): Promise<boolean> {
            const userAuthenticated = await authApi.checkAuthenticated();
            setUser(userAuthenticated);
            return userAuthenticated !== undefined;
        }

        checkIsAuthenticated()
            .catch((error) => console.error("Error checking if user is authenticated", error))
            .finally(() => setInitialAuthLoading(false));
    }, []);


    return (
        <AuthContext.Provider value={{
            user,
            logIn,
            logOut,
            signUp,
            isAuthLoading: authLoading,
            isInitialAuthLoading: initialAuthLoading,
        }}>
            {props.children}
        </AuthContext.Provider>
    );
}
