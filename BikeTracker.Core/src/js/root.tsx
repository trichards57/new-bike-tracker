import * as React from 'react';
import { MainNav } from './navbar';
import { Home } from './home/home';
import { Login } from './login/login';
import { BrowserRouter as Router, Route } from 'react-router-dom';
import * as Cookies from 'js-cookie';

interface IRootProps {
}

interface IRootState {
    authenticated: boolean;
    name?: string;
}

interface IWhoAmIResult {
    authenticated: boolean;
    realName: string;
    role: string[];
    userName: string;
}

export class Root extends React.Component<IRootProps, IRootState> {
    constructor(props: IRootProps) {
        super(props);

        this.state = {
            authenticated: true
        };

        this.checkAuthentication();
    }

    async checkAuthentication() {
        let result = await fetch('/api/account/whoami', {
            credentials: 'same-origin'
        });

        let whoAmI = await result.json() as IWhoAmIResult;

        this.setState({
            authenticated: whoAmI.authenticated,
            name: whoAmI.realName
        });
    }

    async logOut() {
        let headers = new Headers();

        headers.append("RequestVerificationToken", Cookies.get('XSRF-TOKEN'));

        let result = await fetch('/account/logout', {
            credentials: 'same-origin',
            method: 'POST',
            headers
        });

        location.reload(true);
    }

    render() {
        return (
            <Router>
                <div>
                    <MainNav name={this.state.name} authenticated={this.state.authenticated} onLogout={() => this.logOut()} />
                    <Route exact path='/' component={Home} />
                    <Route exact path='/app/login' component={Login} />
                </div>
            </Router>);
    }
}
