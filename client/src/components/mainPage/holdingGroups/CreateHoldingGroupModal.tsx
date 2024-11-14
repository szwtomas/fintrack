import {Form, Input, Modal} from "antd";

export interface CreateHoldingGroupModalProps {
    isOpen: boolean;
    onOk: () => void;
    onCancel: () => void;
    holdingGroupName: string;
    setHoldingGroupName: (name: string) => void;
}

export function CreateHoldingGroupModal(props: CreateHoldingGroupModalProps) {
    return (
        <Modal title="Crear grupo de activos" open={props.isOpen} onOk={props.onOk} onCancel={props.onCancel}>
            <Form name="create holding groups">
                <Form.Item label="Nombre">
                    <Input
                        value={props.holdingGroupName}
                        onChange={(e) => props.setHoldingGroupName(e.target.value)}
                    />
                </Form.Item>
            </Form>
        </Modal>
    );
}