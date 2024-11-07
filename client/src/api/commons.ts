import {getBaseUrl} from "./hostGetter.ts";

export async function makePost(path: string, body: unknown): Promise<Response> {
    return await fetch(`${getBaseUrl()}${path}`, {
        method: "POST",
        headers: {"Content-Type": "application/json"},
        credentials: "include",
        body: JSON.stringify(body),
    });
}

export async function makeGet(path: string): Promise<Response> {
    return await fetch(`${getBaseUrl()}${path}`, {
        method: "GET",
        headers: {"Content-Type": "application/json"},
        credentials: "include",
    });
}
