import {User} from "../context/AuthContext.tsx";
import {makeGet, makePost} from "./commons.ts";

export class InvalidCredentialsException extends Error {
    constructor() {
        super("Invalid credentials");
    }
}

export async function checkAuthenticated(): Promise<User | undefined> {
    const res = await makeGet("/auth");
    if (res.ok) {
        const userJson = await res.json();
        return userJson as User;
    } else {
        return undefined;
    }
}

export async function logIn(email: string, password: string): Promise<User> {
    const res = await makePost("/auth/logIn", {email, password});
    if (res.ok) {
        const userJson = await res.json();
        return userJson as User;
    } else if (res.status === 401) {
        throw new InvalidCredentialsException();
    } else {
        throw new Error(`Unexpected exception logging in, status code ${res.status}`);
    }
}

export async function signUp(email: string, password: string): Promise<void> {
    const body = {email, password};
    const res = await makePost("/auth/signUp", body);
    if (res.status === 201) {
        return undefined;
    } else {
        throw new Error(`Unexpected exception logging with status code ${res.status}`);
    }
}

export async function logOut(): Promise<void> {
    const response = await makePost("/auth/logOut", {});
    if (!response.ok) {
        throw new Error(`Error logging out, received status code ${response.status} from server`);
    }
}
