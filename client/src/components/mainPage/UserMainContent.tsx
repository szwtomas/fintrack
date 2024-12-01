import {Button, Spin, Typography} from "antd";
import {PlusOutlined} from "@ant-design/icons";
import {useEffect, useState} from "react";
import {createHoldingGroup, HoldingGroup} from "../../api/holdingGroupApi.ts";
import {CreateHoldingGroupModal} from "./holdingGroups/CreateHoldingGroupModal.tsx";
import {Holding} from "../../api/holdingApi.ts";
import {getHoldingGroupsWithHoldings} from "../../services/holdingGroupWithHoldingsService.ts";
import {HoldingGroupsCollapse} from "./holdingGroups/HoldingGroupsCollapse.tsx";

export interface HoldingGroupWithHoldings extends HoldingGroup {
    holdings: Holding[];
}

export function UserMainContent() {
    const [holdingGroups, setHoldingGroups] = useState<HoldingGroupWithHoldings[]>([]);
    const [newHoldingGroupName, setNewHoldingGroupName] = useState<string>("");
    const [createHoldingGroupLoading, setCreateHoldingGroupLoading] = useState<boolean>(false);
    const [getGroupsLoading, setGroupsLoading] = useState<boolean>(false);
    const [createHoldingGroupModalOpen, setCreateHoldingGroupModalOpen] = useState<boolean>(false);

    const fetchHoldingGroups = () => {
        setGroupsLoading(true);
        getHoldingGroupsWithHoldings()
            .then((holdingGroups) => setHoldingGroups(holdingGroups))
            .catch(console.error)
            .finally(() => setGroupsLoading(false));
    }

    useEffect(() => {
        fetchHoldingGroups();
    }, []);

    async function onCreateHoldingGroup() {
        setCreateHoldingGroupLoading(true);
        setCreateHoldingGroupModalOpen(false);
        createHoldingGroup(newHoldingGroupName)
            .then(() => setNewHoldingGroupName(""))
            .catch(() => console.error("Error creating holding group"))
            .finally(() => setCreateHoldingGroupLoading(false));
        fetchHoldingGroups();
    }

    if (getGroupsLoading) {
        return (
            <>
                <div>Loading...</div>
                <Spin/>
            </>
        );
    }

    return (
        <div>
            <Typography.Title>Activos</Typography.Title>
            <Button
                type={"primary"}
                icon={<PlusOutlined/>}
                onClick={() => setCreateHoldingGroupModalOpen(true)}
                loading={createHoldingGroupLoading}
            >
                Crear Grupo de Activos
            </Button>
            <CreateHoldingGroupModal
                isOpen={createHoldingGroupModalOpen}
                onOk={onCreateHoldingGroup}
                onCancel={() => setCreateHoldingGroupModalOpen(false)}
                holdingGroupName={newHoldingGroupName}
                setHoldingGroupName={setNewHoldingGroupName}
            />
            <HoldingGroupsCollapse holdingGroups={holdingGroups}/>
        </div>
    );
}