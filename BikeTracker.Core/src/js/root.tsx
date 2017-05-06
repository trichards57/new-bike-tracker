import * as React from 'react';
import { MainNav } from './navbar';
import { Home } from './home/home';
import { Login } from './login/login';
import { BrowserRouter as Router, Route } from 'react-router-dom';
import * as Cookies from 'js-cookie';
import { AppStore } from "./store/app-store";
import { observer } from "mobx-react";
import { Alert, Container } from "reactstrap";

interface IRootProps {
    store: AppStore;
}



@observer
export class Root extends React.Component<IRootProps, undefined> {
    constructor(props: IRootProps) {
        super(props);

        this.props.store.user.checkAuthentication(true);
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
        let showError = this.props.store.error.length > 0;

        return (
            <Router>
                <div>
                    <MainNav name={this.props.store.user.name} authenticated={this.props.store.user.authenticated} onLogout={() => this.logOut()} />
                    <Container>
                        <Alert isOpen={showError} color="danger" toggle={() => this.props.store.clearError()}>
                            {this.props.store.error}
                        </Alert>
                    </Container>
                    <Route exact path='/' component={Home} />
                    <Route exact path='/app/login' component={Login} />
                </div>
            </Router>);
    }
}
