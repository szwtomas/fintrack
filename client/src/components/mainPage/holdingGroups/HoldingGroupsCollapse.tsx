import {Collapse} from "antd";

export function HoldingGroupsCollapse() {
    return (
        <div style={{marginTop: "12px"}}>
            <Collapse>
                <Collapse.Panel header="Balanz" key="1">
                    <p>Contenido</p>
                </Collapse.Panel>
                <Collapse.Panel header="HSBC Argentina" key="1">
                    <p>Contenido</p>
                </Collapse.Panel>
                <Collapse.Panel header="HSBC Cuenta US" key="1">
                    <p>Contenido</p>
                </Collapse.Panel>
                <Collapse.Panel header="Efectivo" key="1">
                    <p>Contenido</p>
                </Collapse.Panel>
                <Collapse.Panel header="RSUs ETrade" key="1">
                    <p>Contenido</p>
                </Collapse.Panel>
            </Collapse>
        </div>
    );
}