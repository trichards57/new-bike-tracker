import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { Navbar, NavbarToggler, NavbarBrand, Collapse } from 'reactstrap';
import { Form, FormGroup, InputGroup, Input, Container } from 'reactstrap';
import { AdminNav } from "./navbar";
import { AppStore } from "../store/app-store";
import { inject, observer } from "mobx-react";
import { LoadingDisplay } from "../shared/loading-display";
import { UserTable } from "./user-table";

interface IAdminProps extends RouteComponentProps<any> {
    appStore: AppStore
}

@inject("appStore") @observer
export class Admin extends React.Component<IAdminProps, undefined> {
    componentWillMount() {
        if (!this.props.appStore.user.authenticated || !this.props.appStore.user.administrator) {
            this.props.history.push("/app/login");
        }
    }

    componentDidMount() {
        this.props.appStore.admin.loadUsers();
    }

    render() {
        let content: any;

        if (this.props.appStore.admin.loadingUsers) {
            content = <LoadingDisplay />
        }
        else {
            content = <UserTable users={this.props.appStore.admin.users} />
        }

        return (
            <Container>
                <AdminNav />
                {content}
            </Container>
        );
    }
}