import * as React from 'react';
import { MainNav } from './navbar';
import { Home } from './home/home';
import { Admin } from './admin/admin';
import { Login } from './login/login';
import { BrowserRouter as Router, Route, RouteProps } from 'react-router-dom';
import * as Cookies from 'js-cookie';
import { AppStore } from "./store/app-store";
import { observer } from "mobx-react";
import { Alert, Container } from "reactstrap";
import { RouteComponentProps, Redirect } from "react-router";
import { LoadingDisplay } from "./shared/loading-display";

interface IRootProps {
    store: AppStore;
}

interface IRootState {
    loading: boolean;
}

@observer
export class Root extends React.Component<IRootProps, IRootState> {
    constructor(props: IRootProps) {
        super(props);

        this.state = {
            loading: true
        };
    }

    componentWillMount() {
        this.checkLogin();
    }

    async checkLogin() {
        await this.props.store.user.checkAuthentication(true);

        console.log("Authentication Returned, cancelling loading page.");

        this.setState({
            loading: false
        });
    }

    render() {
        let showError = this.props.store.error.length > 0;

        if (this.state.loading) {
            return <LoadingDisplay />
        }

        return (
            <Router>
                <div>
                    <MainNav name={this.props.store.user.name} authenticated={this.props.store.user.authenticated}
                        administrator={this.props.store.user.administrator} />
                    <Container>
                        <Alert isOpen={showError} color="danger" toggle={() => this.props.store.clearError()}>
                            {this.props.store.error}
                        </Alert>
                    </Container>
                    <Route exact path='/' component={Home} />
                    <Route exact path='/app/login' component={Login} />
                    <Route exact path='/app/admin' component={Admin} />
                </div>
            </Router>);
    }
}
