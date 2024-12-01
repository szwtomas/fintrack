import {HoldingGroupWithHoldings} from "../UserMainContent.tsx";
import {Button} from "antd";

export interface HoldingGroupPanelContentProps {
    holdingGroup: HoldingGroupWithHoldings;
}

export function HoldingGroupPanelContent(props: HoldingGroupPanelContentProps) {
    if (props.holdingGroup.holdings.length === 0) {
        return (
            <Button variant={"filled"} color={"primary"}>Agregar Activo</Button>
        );
    }

    return (
        <div>
            {props.holdingGroup.holdings.map((holding) => {
                return (
                    <div key={holding.holdingId}>
                        HoldingId - {holding.holdingId}
                    </div>
                );
            })}
        </div>
    );
}