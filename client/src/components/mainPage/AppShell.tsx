import {Layout, theme} from "antd";
import {SideContent} from "./sider/SideContent.tsx";
import React from "react";

export interface AppShellProps {
    content: React.ReactNode;
}

export function AppShell(props: AppShellProps) {
    const {token: {colorBgContainer, borderRadiusLG}} = theme.useToken();

    return (
        <Layout style={{height: "100vh", width: "100vw"}}>
            <Layout.Sider
                width={"200px"}
                theme="light"
                breakpoint="lg"
                collapsedWidth="0"
            >
                <SideContent/>
            </Layout.Sider>
            <Layout>
                <Layout.Content style={{margin: '24px 16px 0'}}>
                    <div
                        style={{
                            padding: 24,
                            minHeight: 360,
                            background: colorBgContainer,
                            borderRadius: borderRadiusLG,
                            width: "100%",
                            height: "100%",
                        }}
                    >
                        {props.content}
                    </div>
                </Layout.Content>
                <Layout.Footer style={{textAlign: 'center'}}>
                    FinTrack ©{new Date().getFullYear()} Created by Tomás Szwarcberg
                </Layout.Footer>
            </Layout>
        </Layout>
    );
}
