import React from 'react';
import { MainNav } from './navbar.jsx';
import { Home } from './home/home.jsx';
import { Login } from './login/login.jsx';
import { BrowserRouter as Router, Route } from 'react-router-dom';

export class Root extends React.Component {
    render() {
        return (
            <Router>
                <div>
                    <MainNav />
                    <Route exact path='/' component={Home} />
                    <Route exact path='/app/login' component={Login} />
                </div>
            </Router>);

    }
}
