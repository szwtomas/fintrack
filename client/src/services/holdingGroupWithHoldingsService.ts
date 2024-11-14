import {HoldingGroupWithHoldings} from "../components/mainPage/UserMainContent.tsx";
import {getHoldingGroups, getHoldingsForHoldingGroup, HoldingGroup} from "../api/holdingGroupApi.ts";
import {Holding} from "../api/holdingApi.ts";

export async function getHoldingGroupsWithHoldings(): Promise<HoldingGroupWithHoldings[]> {
    const holdingGroups: HoldingGroup[] = await getHoldingGroups();
    const holdingGroupsWithHoldings: HoldingGroupWithHoldings[] = holdingGroups.map((holdingGroup) => {
        return {...holdingGroup, holdings: []};
    });

    const promises: Promise<Holding[]>[] = [];
    holdingGroupsWithHoldings.forEach(holdingGroup => {
        const promise = getHoldingsForHoldingGroup(holdingGroup.holdingGroupId);
        promises.push(promise);
    });

    const holdingsResult = await Promise.all(promises);
    const holdings = holdingsResult.flat();
    for (const holding of holdings) {
        const holdingGroup = holdingGroupsWithHoldings
            .find((group) => group.holdingGroupId === holding.holdingGroupId);
        if (holdingGroup !== undefined) {
            holdingGroup.holdings.push(holding);
        }
    }

    return holdingGroupsWithHoldings;
}
