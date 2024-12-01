import "./selectedHoldingGroupModal.css";
import {Button, Input, Modal, Space, Typography} from "antd";
import {HoldingGroupWithHoldings} from "../UserMainContent.tsx";
import {useEffect, useState} from "react";

export interface SelectedHoldingGroupModal {
    isOpen: boolean;
    onCancel: () => void;
    onOk: () => void;
    holdingGroup?: HoldingGroupWithHoldings;
}

export function SelectedHoldingGroupModal(props: SelectedHoldingGroupModal) {
    const [isEditNameActive, setIsEditNameActive] = useState<boolean>(false);
    const [editedName, setEditedName] = useState<string>(props.holdingGroup?.name || "");

    useEffect(() => {
        if (props.holdingGroup !== undefined) {
            setEditedName(props.holdingGroup.name);
        }
    }, [props.holdingGroup]);

    function onClickSaveEditName() {
        setIsEditNameActive(false);
    }

    return (
        <Modal
            open={props.isOpen}
            onOk={props.onOk}
            onCancel={props.onCancel}
        >
            {props.holdingGroup !== undefined && !isEditNameActive &&
                <div className="selected-title" onClick={() => setIsEditNameActive(true)} style={{marginTop: "4px"}}>
                    <Typography.Title level={3}>
                        {props.holdingGroup.name}
                    </Typography.Title>
                </div>
            }
            {
                props.holdingGroup !== undefined && isEditNameActive &&
                <div style={{marginTop: "28px", width: "100%"}}>
                    <Space.Compact className={"change-title-name"}>
                        <Input
                            value={editedName}
                            onChange={(e) => setEditedName(e.target.value)}
                        />
                        <Button type="primary" onClick={onClickSaveEditName}>Guardar</Button>
                    </Space.Compact>
                </div>
            }
        </Modal>
    );
}