import React, {Context, createContext, useState} from "react";

export interface User {
    userId: number;
    email: string;
}

interface AuthContextProps {
    user?: User;
    logIn: (email: string, password: string) => Promise<string | undefined>;
    logOut: () => Promise<void>;
    signUp: (email: string, firstName: string, lastName: string, password: string, gender: string, birthday: string) => Promise<string | undefined>;
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
        return undefined;
    }

    async function logOut(): Promise<void> {

    }

    async function signUp(email: string, firstName: string, lastName: string, password: string, gender: string, birthday: string): Promise<string | undefined> {
        return undefined;
    }


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
