import * as React from 'react';
import { LoginBox } from './login-box';
import { RouteComponentProps } from 'react-router-dom';

export class Login extends React.Component<RouteComponentProps<any>, undefined> {
    public render() {
        return (
            <LoginBox onLogin={(u, s) => this.login(u, s)} />
        );
    }

    private async login(username: string, password: string) {
        let headers = new Headers();
        headers.append("Content-Type", "application/json");

        const result = await fetch("/token", {
            method: 'POST',
            body: JSON.stringify({
                username, password
            }),
            headers
        });
    }
}
