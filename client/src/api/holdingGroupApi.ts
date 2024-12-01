import {makeGet, makePost} from "./commons.ts";
import {Holding} from "./holdingApi.ts";

export interface HoldingGroup {
    holdingGroupId: number;
    name: string;
}

export async function getHoldingGroups(): Promise<HoldingGroup[]> {
    const res = await makeGet("/holding-group");
    if (!res.ok) {
        throw new Error("Error fetching holding groups");
    }

    return await res.json();
}

export async function createHoldingGroup(name: string): Promise<void> {
    const res = await makePost("/holding-group", {name});
    if (res.status !== 201) {
        throw new Error("Error creating holding group");
    }

    return;
}

export function getHoldingsForHoldingGroup(holdingGroupId: number): Promise<Holding[]> {
    return makeGet(`/holding-group/${holdingGroupId}/holding`)
        .then((res) => {
            if (!res.ok) {
                throw new Error("Error fetching holdings");
            }

            return res.json();
        });
}