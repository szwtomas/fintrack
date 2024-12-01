import {Button, Collapse, Typography} from "antd";
import {HoldingGroupWithHoldings} from "../UserMainContent.tsx";
import {HoldingGroupPanelContent} from "./HoldingGroupPanelContent.tsx";
import {useState} from "react";
import {SelectedHoldingGroupModal} from "./SelectedHoldingGroupModal.tsx";

export interface HoldingGroupsCollapseProps {
    holdingGroups: HoldingGroupWithHoldings[];
}

export function HoldingGroupsCollapse(props: HoldingGroupsCollapseProps) {
    const [openHoldingGroup, setOpenHoldingGroup] = useState<HoldingGroupWithHoldings | undefined>(undefined);
    if (props.holdingGroups.length === 0) {
        return (
            <Typography.Title level={4}>
                Aún no tienes grupos de Activos! Crea uno clickeando el botón.
            </Typography.Title>
        );
    }

    return (
        <>
            <SelectedHoldingGroupModal
                isOpen={openHoldingGroup !== undefined}
                onOk={() => setOpenHoldingGroup(undefined)}
                onCancel={() => setOpenHoldingGroup(undefined)}
                holdingGroup={openHoldingGroup!}
            />
            <div style={{marginTop: "12px"}}>
                <Collapse>
                    {props.holdingGroups.map((holdingGroup) => {
                        return (
                            <Collapse.Panel
                                key={holdingGroup.holdingGroupId}
                                header={
                                    <div style={{position: "absolute"}}>
                                        <div style={{position: "relative", top: "-4px", left: "-10px"}}>
                                            <Button
                                                onClick={() => setOpenHoldingGroup(holdingGroup)}
                                                color={"primary"}
                                                variant={"text"}
                                            >
                                                {holdingGroup.name}
                                            </Button>
                                        </div>
                                    </div>
                                }
                            >
                                <HoldingGroupPanelContent holdingGroup={holdingGroup}/>
                            </Collapse.Panel>
                        );
                    })}
                </Collapse>
            </div>
        </>

    );
}