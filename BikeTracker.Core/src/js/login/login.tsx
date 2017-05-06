import * as React from 'react';
import { LoginBox } from './login-box';
import { RouteComponentProps } from 'react-router-dom';
import { AppStore } from "../store/app-store";
import { inject, observer } from "mobx-react";

interface ILoginProps extends RouteComponentProps<any> {
    appStore : AppStore
}

@inject("appStore") @observer
export class Login extends React.Component<ILoginProps, undefined> {
    public render() {
        return (
            <LoginBox onLogin={(u, s) => this.login(u, s)} />
        );
    }

    private async login(username: string, password: string) {
        let headers = new Headers();
        headers.append("Content-Type", "application/json");

        try {
            const result = await fetch("/token", {
                method: 'POST',
                credentials: 'same-origin',
                body: JSON.stringify({
                    username, password
                }),
                headers
            });

            if (result.ok) {
                this.props.appStore.user.checkAuthentication();

                this.props.history.push("/");
            }
        }
        catch (err) {
            this.props.appStore.showError("Could not contact server.  Are you online?");
        }

    }
}
